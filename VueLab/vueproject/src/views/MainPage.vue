<template>
  <div class="main-page">
    <header class="header">
      <div class="logo">
        <img
          src="D:\repos\GoalTracker\VueLab\vueproject\src\logo.png"
          alt="Logo"
        />
        <div class="app-title">Goal Tracker</div>
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
          @click="activeTab = 'tasks'"
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
        <div class="nav-item" @click="routToTheme">Темы</div>
      </aside>
      <main class="main-content">
        <!-- Вкладка Задачи -->
        <section class="tab-section" :class="{ active: activeTab === 'tasks' }">
          <!-- Плашка фильтрации и поиска -->
          <div class="task-filters">
            <input
              v-model="searchQuery"
              type="text"
              placeholder="Поиск по названию или описанию"
              class="filter-input"
            />

            <select v-model="filterStatus" class="filter-select">
              <option value="">Все статусы</option>
              <option
                v-for="status in statuses"
                :key="status.id"
                :value="status.id"
              >
                {{ status.title }}
              </option>
            </select>

            <select v-model="filterImportance" class="filter-select">
              <option value="">Все приоритеты</option>
              <option v-for="imp in importances" :key="imp.id" :value="imp.id">
                {{ imp.title }}
              </option>
            </select>

            <select v-model="filterReminders" class="filter-select">
              <option value="">Любое количество напоминаний</option>
              <option value="0">Без напоминаний</option>
              <option value="1">1 напоминание</option>
              <option value="more">Больше 1</option>
            </select>

            <select v-model="filterDeadline" class="filter-select">
              <option value="">Любая дата</option>
              <option value="overdue">Прошедшие</option>
              <option value="today">Сегодня</option>
              <option value="upcoming">Будущие</option>
            </select>
          </div>

          <div class="centered">
            <button class="add-task-button" @click="AddTask">
              Добавить задачу
            </button>
          </div>

          <div class="tasks-grid">
            <div
              class="task-card"
              :style="{
                outlineColor: getTaskBackgroundColor(
                  task.important || task.idImportance,
                ),
                outlineWidth: '4px',
                outlineStyle: 'solid',
              }"
              v-for="task in filteredTasks"
              :key="task.id"
            >
              <div class="task-header">
                <h3>{{ task.title }}</h3>
                <span v-if="task.autoImportance" class="very-important">!</span>
              </div>
              <p class="task-description">{{ task.description }}</p>

              <!-- Наказание -->
              <div class="task-deadline" v-if="task.punishment">
                Иначе: <strong>{{ task.punishment }}</strong>
              </div>
              <p></p>
              <p></p>
              <!-- Дедлайн -->
              <div class="task-deadline" v-if="task.deadline">
                Дедлайн: {{ formatDate(task.deadline) }}
              </div>

              <!-- Статус -->
              <div class="task-status" v-if="task.status">
                Статус: <strong>{{ getStatusName(task.status) }}</strong>
              </div>

              <!-- Приоритет -->
              <div class="task-status" v-if="task.important">
                Приоритет:
                <strong>{{ getImportanceName(task.important) }}</strong>
              </div>

              <!-- Напоминания -->
              <div
                class="sendEmail"
                v-if="task.sendEmail && task.sendEmail.length > 0"
              >
                <h4>Напоминания:</h4>
                <ul>
                  <li v-for="reminder in task.sendEmail" :key="reminder.id">
                    {{ formatDate(reminder.date) }}
                  </li>
                </ul>
              </div>
              <p></p>

              <p></p>
              <div>
                <div class="reminders-button-container">
                  <button @click="showsendEmail(task)">Напоминания</button>
                </div>
                <div class="task-actions">
                  <button @click="editGoal(task)">Редактировать</button>
                  <button
                    class="delete-button"
                    @click="confirmDelete(authStore.idUser, task.id)"
                  >
                    Удалить
                  </button>
                </div>
              </div>
            </div>
          </div>
        </section>

        <!-- Вкладка Проекты -->
        <section
          class="tab-section"
          :class="{ active: activeTab === 'projects' }"
        >
          <div class="placeholder">Здесь будут проекты.</div>
        </section>

        <!-- Вкладка Настройки (Темы) -->

        <!-- Модальное окно напоминаний -->
        <div v-if="showReminderModal" class="modal-overlay">
          <div class="modal modal-reminders">
            <h3>Напоминания для "{{ currentTask?.title }}"</h3>

            <!-- Форма добавления напоминания -->
            <div class="reminder-form">
              <input type="datetime-local" v-model="newReminderDate" />
              <textarea
                v-model="newReminderText"
                placeholder="Текст напоминания"
                class="reminder-text"
              ></textarea>
              <button @click="addReminder">Добавить</button>
            </div>

            <!-- Список напоминаний -->
            <div
              class="reminder-list-container"
              v-if="currentTask.sendEmail.length > 0"
            >
              <ul class="reminder-list">
                <li
                  v-for="(reminder, index) in currentTask.sendEmail"
                  :key="reminder.id"
                  class="reminder-item"
                >
                  <div class="reminder-header">
                    {{ formatDate(reminder.date) }}
                  </div>
                  <div class="reminder-message">
                    {{ reminder.message || "Без текста" }}
                  </div>
                  <div class="reminder-actions">
                    <button @click="removeReminder(index)">Удалить</button>
                  </div>
                </li>
              </ul>
            </div>

            <div v-else class="no-reminders">Нет напоминаний.</div>
            <p></p>
            <!-- Кнопки действия -->
            <div class="modal-actions">
              <button @click="savesendEmail">Сохранить</button>
              <button @click="closeReminderModal">Закрыть</button>
            </div>
          </div>
        </div>

        <!-- Модальное окно создания/редактирования цели -->
        <div v-if="showGoalModal" class="modal-overlay">
          <div class="modal modal-goal">
            <h3>
              {{ goalForm.id ? "Редактировать цель" : "Создать новую цель" }}
            </h3>

            <div class="goal-form">
              <!-- Название -->
              <label>Название:</label>
              <input
                type="text"
                v-model="goalForm.title"
                placeholder="Введите название"
                class="input-field"
              />

              <!-- Описание -->
              <label>Описание:</label>
              <textarea
                v-model="goalForm.description"
                placeholder="Введите описание"
                class="textarea-field"
              ></textarea>

              <!-- Статус -->
              <label>Статус:</label>
              <select v-model="goalForm.status" class="select-field">
                <option value="">Выберите статус</option>
                <option
                  v-for="status in statuses"
                  :key="status.id"
                  :value="status.id"
                >
                  {{ status.title }}
                </option>
              </select>

              <!-- Приоритет -->
              <label>Приоритет:</label>
              <select v-model="goalForm.important" class="select-field">
                <option value="">Выберите приоритет</option>
                <option
                  v-for="imp in importances"
                  :key="imp.id"
                  :value="imp.id"
                >
                  {{ imp.title }}
                </option>
              </select>

              <!-- Наказание -->
              <label>Наказание:</label>
              <input
                type="text"
                v-model="goalForm.punishment"
                placeholder="Введите наказание"
                class="input-field"
              />

              <!-- Дедлайн -->
              <label>Дедлайн:</label>
              <input
                type="datetime-local"
                v-model="goalForm.deadline"
                class="input-field"
              />

              <!-- AutoImportance -->
              <div class="checkbox-container">
                <input
                  type="checkbox"
                  id="autoImportance"
                  v-model="goalForm.autoImportance"
                />
                <label for="autoImportance">Автоназначение приоритета</label>
              </div>
            </div>

            <p></p>
            <!-- Кнопки действия -->
            <div class="modal-actions">
              <button @click="saveGoal">
                {{ goalForm.id ? "Сохранить изменения" : "Создать" }}
              </button>
              <button @click="closeGoalModal">Отмена</button>
            </div>
          </div>
        </div>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, h } from "vue";
import { useAuthStore, useThemeStore } from "@/store";
import { decodeJwtToken } from "@/utils/jwt";
import router from "@/router/script";
import { toast } from "vue3-toastify";
import { deleteCookie } from "@/utils/jwt";
import {
  fetchUserGoals,
  fetchImportances,
  fetchStatuses,
  fetchUpdateGoal,
  fetchCreateGoal,
  fetchDeleteGoal,
} from "@/services/api";
import { message } from "ant-design-vue";
import "@/assets/styles.css";

const authStore = useAuthStore();
const activeTab = ref("tasks");
const tasks = ref([]);
const email = authStore.email;
const statuses = ref([]);
const importances = ref([]);

onMounted(async () => {
  const decoded = decodeJwtToken();
  if (!decoded) {
    router.push({ path: "/loginpage" });
    toast.error("Пожалуйста, войдите в систему");
    return;
  }

  try {
    await loadGoals();
    statuses.value = await fetchStatuses();
    importances.value = await fetchImportances();
    sessionStorage.setItem("statuses", JSON.stringify(statuses.value));
    sessionStorage.setItem("importances", JSON.stringify(importances.value));
  } catch (error) {
    console.error("Ошибка загрузки данных:", error);
    toast.error("Не удалось загрузить данные");
  }
});

async function loadGoals() {
  const fetchedTasks = await fetchUserGoals(authStore.idUser);
  tasks.value = fetchedTasks.map((task) => ({
    id: task.id,
    title: task.title,
    description: task.description || "",
    important: task.idImportance,
    deadline: task.deadline,
    status: task.idStatus,
    sendEmail: Array.isArray(task.sendEmail) ? task.sendEmail : [],
    punishment: task.punishment,
    idUser: task.idUser,
    autoImportance: task.AutoImportance || false,
  }));
}

function getStatusName(statusId) {
  const status = statuses.value.find((s) => s.id === statusId);
  return status ? status.title : "Неизвестный статус";
}

function getImportanceName(ImportanceId) {
  const importance = importances.value.find((s) => s.id === ImportanceId);
  return importance ? importance.title : "Неизвестный приоритет";
}

function logout() {
  authStore.clearSessionStorage();
  authStore.$reset(); 
  deleteCookie("goida");
  router.push("/loginpage");
  toast.success("Вы успешно вышли");
}

const formatDate = (isoDate) => {
  if (!isoDate) return "";
  const date = new Date(isoDate);
  return (
    date.toLocaleDateString() +
    " " +
    date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })
  );
};

const themeStore = useThemeStore();

function getTaskBackgroundColor(idImportance) {
  const theme = themeStore.importanceThemes.find(
    (t) => t.idImportance === idImportance,
  );
  return theme ? theme.backgroundColor : "#cccccc";
}

function getTaskTextColor(idImportance) {
  const theme = themeStore.importanceThemes.find(
    (t) => t.idImportance === idImportance,
  );
  return theme ? theme.textColor : "#333333";
}

// ==== МОДАЛЬНОЕ ОКНО ДЛЯ СОЗДАНИЯ / РЕДАКТИРОВАНИЯ ЦЕЛИ ====
const showGoalModal = ref(false);
const goalForm = ref({
  id: null,
  title: "",
  description: "",
  status: "",
  important: "",
  punishment: "",
  deadline: "",
  autoImportance: false,
  idUser: authStore.idUser,
});

function AddTask() {
  // Форма с пустыми полями надо а то иначе смерть
  goalForm.value = {
    id: null,
    title: "",
    description: "",
    status: "",
    important: "",
    punishment: "",
    deadline: null,
    autoImportance: false,
    idUser: authStore.idUser,
  };
  showGoalModal.value = true;
}

function editGoal(task) {
  console.log("Редактирование задачи:", task);
  goalForm.value = {
    id: task.id,
    title: task.title,
    description: task.description,
    status: task.status,
    important: task.important || "", 
    punishment: task.punishment || null,
    deadline: task.deadline ? task.deadline.slice(0, 16) : null,
    autoImportance: task.autoImportance || false,
    idUser: task.idUser,
    startDate: task.startDate || getLocalISOTime(new Date()),
  };
  showGoalModal.value = true;
}

async function confirmDelete(userId, taskId) {
  const vNode = h(
    "div",
    {
      style: {
        display: "flex",
        flexDirection: "column",
        gap: "10px",
        padding: "10px",
        borderRadius: "8px",
        backgroundColor: "var(--color-background-soft)",
        color: "var(--color-text)",
        boxShadow: "0 2px 6px rgba(0, 0, 0, 0.1)",
        minWidth: "250px",
        maxWidth: "400px",
        fontSize: "14px",
      },
    },
    [
      "Вы уверены, что хотите удалить эту задачу?",
      h("div", { style: { display: "flex", gap: "10px", marginTop: "10px" } }, [
        h(
          "button",
          {
            class: "toast-confirm",
            style: {
              background: "var(--color-button)",
              color: "var(--color-button-text)",
              border: "none",
              padding: "8px 12px",
              borderRadius: "6px",
              cursor: "pointer",
              transition: "background-color 0.2s ease",
              flex: 1,
              fontWeight: "bold",
            },
            onClick: async () => {
              await deleteGoal(userId, taskId);
              toast.remove(toastId);
            },
          },
          "Удалить",
        ),
        h(
          "button",
          {
            class: "toast-cancel",
            style: {
              background: "transparent",
              color: "var(--color-text)",
              border: "1px solid var(--color-border)",
              padding: "8px 12px",
              borderRadius: "6px",
              cursor: "pointer",
              transition: "background-color 0.2s ease, color 0.2s ease",
              flex: 1,
            },
            onMouseEnter: (e) => {
              e.target.style.backgroundColor = "var(--color-background-mute)";
              e.target.style.color = "var(--color-heading)";
            },
            onMouseLeave: (e) => {
              e.target.style.backgroundColor = "transparent";
              e.target.style.color = "var(--color-text)";
            },
            onClick: () => {
              toast.info("Удаление отменено.");
              toast.remove(toastId);
            },
          },
          "Отмена",
        ),
      ]),
    ],
  );

  const toastId = toast(vNode, {
    type: "warning",
    position: "top-right",
    autoClose: false,
    closeButton: false,
    closeOnClick: false,
    draggable: true,
    pauseOnHover: true,
    theme: "auto",
  });
}

function getLocalISOTime(date) {
  const pad = (n) => (n < 10 ? "0" + n : n);

  const YYYY = date.getFullYear();
  const MM = pad(date.getMonth() + 1);
  const DD = pad(date.getDate());
  const hh = pad(date.getHours());
  const mm = pad(date.getMinutes());
  const ss = pad(date.getSeconds());

  return `${YYYY}-${MM}-${DD}T${hh}:${mm}:${ss}`;
}

async function saveGoal() {
  if (!goalForm.value.title || !goalForm.value.status) {
    message.error("Заполните, пожалуйста, поля: название и статус.");
    return;
  } else if (!goalForm.value.important && !goalForm.value.autoImportance) {
    message.error(
      "Заполните, пожалуйста, поля: приоритет или автоназначение приоритета.",
    );
    return;
  }

  try {
    if (goalForm.value.id) {
      // ИЗМЕНЕНИЕ ЦЕЛИ
      const updated = {
        id: goalForm.value.id,
        title: goalForm.value.title,
        description: goalForm.value.description,
        idStatus: goalForm.value.status,
        idImportance: goalForm.value.important || null,
        punishment: goalForm.value.punishment,
        deadline: goalForm.value.deadline
          ? getLocalISOTime(new Date(goalForm.value.deadline))
          : null,
        autoImportance: goalForm.value.autoImportance,
        idUser: goalForm.value.idUser,
        sendEmail: goalForm.value.sendEmail || [],
        startDate: goalForm.value.startDate || getLocalISOTime(new Date()),
      };
      console.log("updated:", updated);
      const response = await fetchUpdateGoal(updated);
      console.log(response, "Ответ от API при обновлении цели");
      // Обновляем отображение на стороне клиента
      const idx = tasks.value.findIndex((t) => t.id === goalForm.value.id);
      if (idx !== -1) {
        tasks.value[idx] = {
          ...tasks.value[idx],
          title: goalForm.value.title,
          description: goalForm.value.description,
          status: goalForm.value.status,
          important: response.idImportance,
          punishment: goalForm.value.punishment,
          deadline: goalForm.value.deadline
            ? getLocalISOTime(new Date(goalForm.value.deadline))
            : null,
          autoImportance: goalForm.value.autoImportance,
        };
      }
      toast.success("Задача успешно обновлена");
    } else {
      console.log("goalForm.value.important:", goalForm.value.important);
      // НОВАЯ ЦЕЛЬ
      const toCreate = {
        id: crypto.randomUUID(),
        title: goalForm.value.title,
        description: goalForm.value.description,
        idStatus: goalForm.value.status,
        idImportance: goalForm.value.important,
        punishment: goalForm.value.punishment,
        deadline: goalForm.value.deadline
          ? getLocalISOTime(new Date(goalForm.value.deadline))
          : null,
        AutoImportance: goalForm.value.autoImportance || false,
        idUser: authStore.idUser,
        startDate: getLocalISOTime(new Date()),
        sendEmail: [],
      };
      await console.log("Создание цели:", toCreate);
      const createdTask = await fetchCreateGoal(toCreate);
      console.log("Созданная задача:", createdTask);
      // Обновляем отображение на стороне клиента
      tasks.value.push({
        id: createdTask.id,
        title: toCreate.title,
        description: toCreate.description || "",
        status: toCreate.idStatus,
        important: createdTask.idImportance,
        punishment: toCreate.punishment,
        deadline: toCreate.deadline,
        sendEmail: Array.isArray(toCreate.sendEmail) ? toCreate.sendEmail : [],
        idUser: toCreate.idUser,
        autoImportance: toCreate.AutoImportance || false,
      });
      console.log;
      toast.success("Задача успешно создана");
    }
    closeGoalModal();
  } catch (err) {
    console.error("Ошибка при сохранении цели:", err);
    toast.error("Не удалось сохранить задачу");
  }
}

function closeGoalModal() {
  showGoalModal.value = false;
  goalForm.value = {
    id: null,
    title: "",
    description: "",
    status: "",
    important: "",
    punishment: "",
    deadline: "",
    autoImportance: false,
    idUser: authStore.idUser,
  };
}

async function deleteGoal(idUser, id) {
  try {
    await fetchDeleteGoal(idUser, id);
    tasks.value = tasks.value.filter((t) => t.id !== id);
    toast.success("Задача удалена");
  } catch (err) {
    console.error("Ошибка при удалении цели:", err);
    toast.error("Не удалось удалить цель");
  }
}

const showReminderModal = ref(false);
const currentTask = ref(null);
const newReminderDate = ref("");
const newReminderText = ref("");

function showsendEmail(task) {
  currentTask.value = task;
  currentTask.value.idImportance = task.important; 
  currentTask.value.idStatus = task.status; я
  newReminderDate.value = "";
  newReminderText.value = "";
  showReminderModal.value = true;
}

function addReminder() {
  if (!newReminderDate.value) return;

  const newReminder = {
    id: crypto.randomUUID(),
    date: newReminderDate.value,
    message: newReminderText.value || "",
    sended: false,
  };

  currentTask.value.sendEmail.push(newReminder);
  newReminderDate.value = "";
  newReminderText.value = "";
}

function removeReminder(index) {
  currentTask.value.sendEmail.splice(index, 1);
}

async function savesendEmail() {
  try {
    await fetchUpdateGoal(currentTask.value);
    const idx = tasks.value.findIndex((t) => t.id === currentTask.value.id);
    if (idx !== -1) {
      tasks.value[idx].sendEmail = [...currentTask.value.sendEmail];
    }
    closeReminderModal();
    toast.success("Напоминания сохранены");
  } catch (err) {
    console.error("Ошибка при сохранении напоминаний:", err);
    toast.error("Не удалось сохранить напоминания");
  }
}

function closeReminderModal() {
  showReminderModal.value = false;
  currentTask.value = null;
  newReminderDate.value = "";
  newReminderText.value = "";
}

const searchQuery = ref("");
const filterStatus = ref("");
const filterImportance = ref("");
const filterReminders = ref("");
const filterDeadline = ref("");

const filteredTasks = computed(() => {
  return tasks.value.filter((task) => {
    const matchesSearch =
      (task.title || "")
        .toLowerCase()
        .includes(searchQuery.value.toLowerCase()) ||
      (task.description || "")
        .toLowerCase()
        .includes(searchQuery.value.toLowerCase());

    const matchesStatus =
      !filterStatus.value || task.status === filterStatus.value;

    const matchesImportance =
      !filterImportance.value || task.important === filterImportance.value;

    const reminderCount = task.sendEmail?.length || 0;
    const matchesReminders =
      !filterReminders.value ||
      (filterReminders.value === "0" && reminderCount === 0) ||
      (filterReminders.value === "1" && reminderCount === 1) ||
      (filterReminders.value === "more" && reminderCount > 1);

    const now = getLocalISOTime(new Date());
    const deadlineDate = task.deadline
      ? getLocalISOTime(new Date(task.deadline))
      : null;
    const matchesDeadline =
      !filterDeadline.value ||
      (filterDeadline.value === "today" &&
        deadlineDate &&
        deadlineDate.toDateString() === now.toDateString()) ||
      (filterDeadline.value === "overdue" &&
        deadlineDate &&
        deadlineDate < now) ||
      (filterDeadline.value === "upcoming" &&
        deadlineDate &&
        deadlineDate > now);

    return (
      matchesSearch &&
      matchesStatus &&
      matchesImportance &&
      matchesReminders &&
      matchesDeadline
    );
  });
});

function routToTheme() {
  router.push({ path: "/themestore" });
}
</script>
