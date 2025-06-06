<template>
  <div class="main-page">
    <header class="header">
      <div class="logo">
        <img src="@\logo.png" alt="Logo" @click="routToGoals"/>
        <div class="app-title" @click="routToGoals">Goal Tracker</div>
      </div>
      <div class="user-info">
        <span>{{ email || "Пользователь" }}</span>
        <button class="logout-button" @click="logout">Выйти</button>
      </div>
    </header>
    <div class="content">
      <aside class="sidebar">
        <div
          class="nav-item"
          :class="{ active: activeTab === 'tasks' }"
          @click="routToGoals"
        >
          Задачи
        </div>
        <div
          class="nav-item"
          :class="{ active: activeTab === 'projects' }"
          @click="activeTab = 'projects'"
        >
          Колесо
        </div>
        <div
          class="nav-item"
          :class="{ active: activeTab === 'settings' }"
          @click="routToThemes"
        >
          Темы
        </div>
      </aside>
      <main class="main-content">
        <!-- Вкладка Задачи -->
        <section class="tab-section" :class="{ active: activeTab === 'tasks' }">
          <div class="placeholder">Здесь будут задачи.</div>
        </section>

        <!-- Вкладка Проекты -->
        <section
          class="tab-section"
          :class="{ active: activeTab === 'projects' }"
        >
          <div class="wheel-container">
            <canvas id="wheel" width="400" height="400"></canvas>
            <button id="spin-btn">Крутить!</button>
          </div>
        
          <p id="result" class="result-p"></p>
          <div class="tasks-list">
    <h3 class="tasks-title" >Выберите задачи для колеса:</h3>
    <ul>
      <li v-for="(task, index) in wheelTasks" :key="index" class="task-item">
        <label>
          <input
            type="checkbox"
            :checked="isSelected(task)"
            @change="toggleTaskSelection(task)"
          />
          {{ task.title }}
        </label>
      </li>
    </ul>
  </div>
        </section>

        <!-- Вкладка Темы -->
        <section
          class="tab-section"
          :class="{ active: activeTab === 'settings' }"
        ></section>
      </main>
    </div>
  </div>
</template>

<script setup>
import "@/assets/styles.css";
import { onMounted, ref, h, computed } from "vue";
import router from "../router/script";
import { useAuthStore, useThemeStore } from "@/store";
import { deleteCookie } from "@/utils/jwt";
import { toast } from "vue3-toastify";
import { message } from "ant-design-vue";
import {
  fetchUserGoals
} from "@/services/api";
import { decodeJwtToken } from "@/utils/jwt";
const wheelTasks = ref([]);
const newWheelTask = ref("");   
const authStore = useAuthStore();
const themeStore = useThemeStore();
const email = authStore.email;
const activeTab = ref("projects");
const drawWheelRef = ref(null);
const drawPointRef = ref(null);

function logout() {
  authStore.clearSessionStorage();
  authStore.$reset();
  deleteCookie("goida");
  router.push("/loginpage");
  toast.success("Вы успешно вышли");
}

function routToGoals() {
  router.push({ path: "/goals" });
}

function routToThemes() {
  router.push({ path: "/themestore" });
}
function getCssVariableValue(variableName) {
  return getComputedStyle(document.documentElement).getPropertyValue(variableName).trim();
}
onMounted( async () => {
  const canvas = document.getElementById("wheel");
  if (!canvas) {
    console.error("Canvas не найден!");
    return;
  }
console.log(authStore.idUser);
  const userGoals = await fetchUserGoals(authStore.idUser);
  wheelTasks.value = userGoals.map(goal => ({
    id: goal.id,
    title: goal.title,
    deadline: goal.deadline,
    autoImportance: goal.autoImportance,
    idImportance: goal.idImportance
  }));

  const ctx = canvas.getContext("2d");
  const spinBtn = document.getElementById("spin-btn");
  const result = document.getElementById("result");

  const prizes = selectedTasks.value;
  const colors = [
    "#FFD700",
    "#C0C0C0",
    "#CD7F32",
    "#FF6347",
    "#6A5ACD",
    "#20B2AA",
  ];
  const size = canvas.width / 2;
  let currentAngle = 0;
  function drawWheel(prizes) {
  ctx.clearRect(0, 0, canvas.width, canvas.height);
  const angleStep = (2 * Math.PI) / prizes.length;

  if (prizes.length === 0) {
    ctx.font = "20px sans-serif";
    const accentColor = getCssVariableValue('--color-accent');
ctx.fillStyle = accentColor;
    ctx.fillText("Выберите задачи", size - 70, size);
    return;
  }

  currentAngle = 0;

  prizes.forEach((task, i) => {
    ctx.beginPath();
    ctx.moveTo(size, size);
    console.log("AAAAAAAAAA")
    ctx.fillStyle = themeStore.importanceThemes.find((t) => t.idImportance === task.idImportance)?.backgroundColor || colors[i % colors.length];
    ctx.arc(size, size, size, currentAngle, currentAngle + angleStep);
    ctx.lineTo(size, size);
    ctx.fill();

    ctx.save();
    ctx.translate(size, size);
    ctx.rotate(currentAngle + angleStep / 2);
    ctx.textAlign = "right";
    ctx.fillStyle = "#fff";
    ctx.font = "bold 14px sans-serif";
    ctx.fillText(task.title, size - 10, 5);
    ctx.restore();
    currentAngle += angleStep;
  });

}
function drawPointer() {

  ctx.beginPath();
  ctx.moveTo(canvas.width, size);
  ctx.lineTo(canvas.width - 20, size - 10);
  ctx.lineTo(canvas.width - 20, size + 10);
  ctx.closePath();
  ctx.fillStyle = "#ff0000";
  ctx.fill();
}
  let currentRotationDeg = 0;
  function animateWheel(targetRotation) {
    drawWheel(prizes);
    return new Promise((resolve) => {
      let start = null;
      const duration = 4000;

      function step(timestamp) {
        if (!start) start = timestamp;
        let progress = timestamp - start;
        const t = Math.min(progress / duration, 1);

        const easeOut = t * (2 - t);
        const rotation = targetRotation * easeOut;
        const totalRotation = currentRotationDeg + rotation;

        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.save();
        ctx.translate(size, size);
        ctx.rotate((totalRotation * Math.PI) / 180);
        ctx.translate(-size, -size);
        drawWheel(prizes);
        ctx.restore();
        drawPointer();
        if (progress < duration) {
          requestAnimationFrame(step);
        } else {
          currentRotationDeg = (currentRotationDeg + targetRotation) % 360;
          resolve(currentRotationDeg);
        }
      }

      requestAnimationFrame(step);
    });
  }
  drawWheelRef.value = drawWheel;
  drawPointRef.value = drawPointer;
  drawWheel(prizes);
  drawPointer();


  spinBtn.addEventListener("click", () => {
    const spinAngle = Math.floor(Math.random() * 5 + 5) * 360;
    const finalOffset = Math.random() * 360;
    const totalRotation = spinAngle + finalOffset;

    animateWheel(totalRotation).then((finalAngle) => {
      const normalized = (360 - (finalAngle % 360)) % 360;
      const sectorAngle = 360 / prizes.length;
      const index = Math.floor(normalized / sectorAngle);
      if(prizes.length === 0) return;
      const winningPrize = prizes[index].title;
      result.textContent = `Вы будете: ${winningPrize}`;
    });
  });
});

const selectedTasks = ref([]);


function isSelected(task) {
  return selectedTasks.value.some(t => t.id === task.id);
}

function toggleTaskSelection(task) {
  const index = selectedTasks.value.findIndex(t => t.id === task.id);
  if (index === -1) {
    selectedTasks.value.push({ ...task }); 
  } else {
    selectedTasks.value.splice(index, 1);
  }
  document.getElementById("result").textContent = "";
//   drawWheel(selectedTasks.value); 
  if (drawWheelRef.value) {
    drawWheelRef.value(selectedTasks.value);
  }
    if (drawPointRef.value) {
        drawPointRef.value();
    }
}
</script>
