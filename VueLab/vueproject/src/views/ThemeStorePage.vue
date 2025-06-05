<template>
  <div class="main-page">
    <header class="header">
      <div class="logo">
        <img src="@\logo.png" alt="Logo" />
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
          @click="activeTab = 'settings'"
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
          <div class="placeholder">Здесь будут проекты.</div>
        </section>

        <!-- Вкладка Темы -->
        <section
          class="tab-section"
          :class="{ active: activeTab === 'settings' }"
        >
          <div class="settings-header">
            <div class="tab-nav">
              <button
                class="tab-button"
                :class="{ active: themeTab === 'public' }"
                @click="themeTab = 'public'"
              >
                Публичные темы
              </button>
              <button
                class="tab-button"
                :class="{ active: themeTab === 'own' }"
                @click="themeTab = 'own'"
              >
                Мои темы
              </button>
            </div>
            <button
              v-if="themeTab === 'own'"
              class="add-theme-button"
              @click="openThemeModal()"
            >
              Создать новую тему
            </button>
          </div>
         
          <div class="search-bar">
            <input
              type="text"
              v-model="themeSearchQuery"
              placeholder="Поиск по названию темы..."
              class="input-field"
            />
          </div>
          <!-- Список публичных тем -->
          <div v-if="themeTab === 'public'" class="theme-list">
            <div
              v-for="themeSet in filteredPublicThemes"
              :key="themeSet.id"
              class="theme-item"
            >
              <div class="theme-name">{{ themeSet.theme.name }}</div>
              <div class="theme-buttons">
                <button class="select-button" @click="selectTheme(themeSet.id)">
                  Выбрать
                </button>
                <button class="select-button" @click="cloneTheme(themeSet)">
                  Клонировать
                </button>
              </div>
            </div>
            <div v-if="filteredPublicThemes.length === 0" class="no-themes">
              Нет доступных публичных тем.
            </div>
          </div>

          <!-- Список собственных тем -->
          <div v-if="themeTab === 'own'" class="theme-list">
            <div
              v-for="themeSet in filteredOwnThemes"
              :key="themeSet.id"
              class="theme-item"
            >
              <div class="theme-name">{{ themeSet.theme.name }}</div>
              <div class="theme-buttons">
                <div class="theme-info">
                  <button
                    class="select-button"
                    @click="selectTheme(themeSet.id)"
                  >
                    Выбрать
                  </button>
                </div>
                <div class="theme-actions">
                  <button
                    class="edit-theme-button"
                    @click="editTheme(themeSet)"
                  >
                    Редактировать
                  </button>
                  <button
                    class="delete-theme-button"
                    @click="confirmDeleteTheme(themeSet.id)"
                  >
                    Удалить
                  </button>
                </div>
              </div>
            </div>
            <div v-if="filteredOwnThemes.length === 0" class="no-themes">
              У вас ещё нет собственных тем.
            </div>
          </div>
        </section>

        
        <div v-if="showThemeModal" class="modal-overlay">
          <div class="modal modal-theme">
            <h3>
              {{ themeForm.id ? "Редактировать тему" : "Создать новую тему" }}
            </h3>
            <div class="theme-form">
              <label>Название темы:</label>
              <input
                type="text"
                v-model="themeForm.theme.name"
                placeholder="Введите имя темы"
                class="input-field"
              />
              <div>
                <p></p>
                <input
                  type="checkbox"
                  v-model="themeForm.public"
                  value="false"
                  class="add-importance-button"
                />
                <label for="public"> Публичная тема</label>
              </div>

              <!-- Основные цвета -->
              <div class="theme-form palitra">
                <fieldset class="fieldset-colors">
                  <legend>Основные цвета</legend>

                  <div class="color-input">
                    <label>Фон:</label>
                    <input
                      type="color"
                      v-model="themeForm.theme.backgroundColor"
                    />
                  </div>

                  <div class="color-input">
                    <label>Текст:</label>
                    <input type="color" v-model="themeForm.theme.textColor" />
                  </div>

                  <div class="color-input">
                    <label>Граница:</label>
                    <input type="color" v-model="themeForm.theme.borderColor" />
                  </div>

                  <div class="color-input">
                    <label>Фон карточки:</label>
                    <input
                      type="color"
                      v-model="themeForm.theme.cardBackground"
                    />
                  </div>

                  <div class="color-input">
                    <label>Кнопка:</label>
                    <input type="color" v-model="themeForm.theme.buttonColor" />
                  </div>

                  <div class="color-input">
                    <label>Текст кнопки:</label>
                    <input
                      type="color"
                      v-model="themeForm.theme.buttonTextColor"
                    />
                  </div>

                  <div class="color-input">
                    <label>Акцент:</label>
                    <input type="color" v-model="themeForm.theme.accentColor" />
                  </div>

                  <div class="color-input">
                    <label>Основной цвет:</label>
                    <input
                      type="color"
                      v-model="themeForm.theme.primaryColor"
                    />
                  </div>

                  <div class="color-input">
                    <label>Вторичный цвет:</label>
                    <input
                      type="color"
                      v-model="themeForm.theme.secondaryColor"
                    />
                  </div>

                  <div class="color-input">
                    <label>Тень:</label>
                    <input type="color" v-model="themeForm.theme.shadowColor" />
                  </div>
                </fieldset>

                <!-- Темы важности -->
                <fieldset class="fieldset-importance">
                  <legend>Цвета для приоритетов</legend>
                  <div class="color-input">
                    <label>Низкая важность:</label>
                    <label>Обводка:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[0].backgroundColor"
                    />
                    <label>Текст:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[0].textColor"
                    />
                  </div>
                  <div class="color-input">
                    <label>Средняя важность:</label>
                    <label>Обводка:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[1].backgroundColor"
                    />
                    <label>Текст:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[1].textColor"
                    />
                  </div>
                  <div class="color-input">
                    <label>Важно:</label>
                    <label>Обводка:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[2].backgroundColor"
                    />
                    <label>Текст:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[2].textColor"
                    />
                  </div>
                  <div class="color-input">
                    <label>Очень важно:</label>
                    <label>Обводка:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[3].backgroundColor"
                    />
                    <label>Текст:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[3].textColor"
                    />
                  </div>
                  <div class="color-input">
                    <label>Срочно:</label>
                    <label>Обводка:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[4].backgroundColor"
                    />
                    <label>Текст:</label>
                    <input
                      type="color"
                      v-model="themeForm.importanceThemes[4].textColor"
                    />
                  </div>
                </fieldset>
              </div>
            </div>

            <div class="modal-actions">
              <button @click="saveTheme">
                {{ themeForm.id ? "Сохранить изменения" : "Создать" }}
              </button>
              <button @click="closeThemeModal">Отмена</button>
            </div>
          </div>
        </div>
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
  fetchThemes,
  fetchCreateTheme,
  fetchUpdateTheme,
  fetchDeleteTheme,
} from "@/services/api";

const publicThemes = ref([]);
const ownThemes = ref([]);
const themeSearchQuery = ref("");

const authStore = useAuthStore();
const themeStore = useThemeStore();
const email = authStore.email;
const activeTab = ref("settings");
const themeSets = ref([]); 
loadThemeSets();

const themeTab = ref("public");

function splitThemes() {
  publicThemes.value = themeSets.value.filter((t) => t.public);
  ownThemes.value = themeSets.value.filter((t) => !t.public);
}
function logout() {
  authStore.clearSessionStorage();
  authStore.$reset(); 
  deleteCookie("goida");
  router.push("/loginpage");
  toast.success("Вы успешно вышли");
}

const filteredPublicThemes = computed(() => {
  if (!themeSearchQuery.value) return publicThemes.value;
  const query = themeSearchQuery.value.toLowerCase();
  return publicThemes.value.filter((t) =>
    t.theme.name.toLowerCase().includes(query),
  );
});

const filteredOwnThemes = computed(() => {
  if (!themeSearchQuery.value) return ownThemes.value;
  const query = themeSearchQuery.value.toLowerCase();
  return ownThemes.value.filter((t) =>
    t.theme.name.toLowerCase().includes(query),
  );
});

const themeForm = ref({});
const showThemeModal = ref(false);
async function loadThemeSets() {
  try {
    const sets = await fetchThemes(authStore.idUser);
    console.log("Загруженные темы:", sets);
    themeSets.value = sets;
    splitThemes();
  } catch (err) {
    console.error("Ошибка загрузки тем:", err);
    toast.error("Не удалось загрузить список тем");
  }
}

function selectTheme(id) {
  themeStore.loadTheme(id);
}

function openThemeModal() {
  themeForm.value = {
    id: null,
    theme: {
      id: crypto.randomUUID(),
      name: "",
      backgroundColor: "#ffffff",
      textColor: "#000000",
      borderColor: "#cccccc",
      cardBackground: "#f5f5f5",
      buttonColor: "#007bff",
      buttonTextColor: "#ffffff",
      accentColor: "#17a2b8",
      primaryColor: "#007bff",
      secondaryColor: "#6c757d",
      shadowColor: "#000000",
    },
    importanceThemes: [
      {
        id: crypto.randomUUID(),
        idTheme: crypto.randomUUID(),
        idImportance: "9B481EA3-322D-4B95-B5C3-5E4C0C4A6775",
        backgroundColor: "#ffffff",
        textColor: "#000000",
      },
      {
        id: crypto.randomUUID(),
        idTheme: crypto.randomUUID(),
        idImportance: "9CF029F0-2353-4C2B-94EA-3DEB6CD566A2",
        backgroundColor: "#ffffff",
        textColor: "#000000",
      },
      {
        id: crypto.randomUUID(),
        idTheme: crypto.randomUUID(),
        idImportance: "A8F7DE47-C2D7-4C4D-861D-A3B555B236D0",
        backgroundColor: "#ffffff",
        textColor: "#000000",
      },
      {
        id: crypto.randomUUID(),
        idTheme: crypto.randomUUID(),
        idImportance: "B5AC2A85-4002-460E-ADA2-447CF4B2C764",
        backgroundColor: "#ffffff",
        textColor: "#000000",
      },
      {
        id: crypto.randomUUID(),
        idTheme: crypto.randomUUID(),
        idImportance: "5E760A5D-3750-4466-8489-E4DC3C47B231",
        backgroundColor: "#ffffff",
        textColor: "#000000",
      },
    ],
  };
  showThemeModal.value = true;
}

function editTheme(themeSet) {
  themeForm.value = {
    id: themeSet.id,
    theme: { ...themeSet.theme },
    importanceThemes: themeSet.importanceThemes.map((imp) => ({
      idImportance: imp.idImportance,
      backgroundColor: imp.backgroundColor,
      textColor: imp.textColor,
      idTheme: themeSet.theme.id,
      id: imp.id,
    })),
  };
  showThemeModal.value = true;
}

function confirmDeleteTheme(id) {
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
      "Вы уверены, что хотите удалить эту тему?",
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
              await deleteTheme(id);
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

async function deleteTheme(id) {
  try {
    await fetchDeleteTheme(authStore.idUser, id);

    ownThemes.value = ownThemes.value.filter((t) => t.id !== id);
    themeSets.value = themeSets.value.filter((t) => t.id !== id);
    toast.success("Тема удалена");
    selectTheme("5EC6627F-1F1B-47E6-8EBD-367BC345F702"); // Сбросить текущую тему на тему по умолчанию
  } catch (err) {
    console.error("Ошибка при удалении темы:", err);
    toast.error("Не удалось удалить тему");
  }
}

function addImportanceEntry() {
  themeForm.value.importanceThemes.push({
    idImportance: "",
    backgroundColor: "#ffffff",
    textColor: "#000000",
  });
}

function removeImportanceEntry(index) {
  themeForm.value.importanceThemes.splice(index, 1);
}

async function saveTheme() {
  if (!themeForm.value.theme.name) {
    message.error("Укажите название темы.");
    return;
  }
  const { theme, importanceThemes } = themeForm.value;
  if (!theme.backgroundColor || !theme.textColor) {
    message.error("Заполните основные цвета темы.");
    return;
  }
  for (const imp of importanceThemes) {
    if (!imp.idImportance) {
      message.error("У каждой темы приоритета должен быть установлен ID.");
      return;
    }
  }

  try {
    if (themeForm.value.id) {
      const updatedSet = {
        id: themeForm.value.id,
        idUser: authStore.idUser,
        theme: { ...themeForm.value.theme },
        importanceThemes: [...themeForm.value.importanceThemes],
      };
      console.log("Обновляем тему:", updatedSet);
      console.log("оригинальная тема:", themeForm.value);
      await fetchUpdateTheme(updatedSet, authStore.idUser);
      await loadThemeSets();
      toast.success("Тема успешно обновлена");
      selectTheme(themeForm.value.id);
    } else {
      const toCreate = {
        id: crypto.randomUUID(),
        idUser: authStore.idUser,
        theme: { ...themeForm.value.theme },
        importanceThemes: [...themeForm.value.importanceThemes],
      };
      const response = await fetchCreateTheme(toCreate, authStore.idUser);
      await loadThemeSets();
      toast.success("Тема успешно создана");
      console.log("Создана тема:", response);
      selectTheme(response);
    }
    closeThemeModal();
  } catch (err) {
    console.error("Ошибка при сохранении темы:", err);
    toast.error("Не удалось сохранить тему");
  }
}

function closeThemeModal() {
  showThemeModal.value = false;
  themeForm.value = {
    id: null,

    theme: {
      name: "",
      backgroundColor: "#ffffff",
      textColor: "#000000",
      borderColor: "#cccccc",
      cardBackground: "#f5f5f5",
      buttonColor: "#007bff",
      buttonTextColor: "#ffffff",
      accentColor: "#17a2b8",
      primaryColor: "#007bff",
      secondaryColor: "#6c757d",
      shadowColor: "#000000",
    },
    importanceThemes: [],
  };
}

function routToGoals() {
  router.push({ path: "/goals" });
}

function cloneTheme(themeSet) {
  const cloned = JSON.parse(JSON.stringify(themeSet));

  cloned.id = null;
  cloned.public = false; 
  cloned.theme.id = crypto.randomUUID(); 
  cloned.theme.name += " (Копия)"; 

  themeForm.value = cloned;

  showThemeModal.value = true;
}
</script>
