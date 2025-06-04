// src/store/index.js
import { defineStore } from 'pinia';
import { fetchThemeSet } from '@/services/api';
import { decodeJwtToken } from '@/utils/jwt';
// src/store/index.js

export const useAuthStore = defineStore('auth', {
  state: () => ({
    idUser: null,
    email: null,
    idThemeSet: null,
  }),
  actions: {
    // Инициализация из sessionStorage или JWT
    initializeAuth() {
      // Сначала проверяем sessionStorage
      const storedAuth = sessionStorage.getItem('auth');
      if (storedAuth) {
        const { idUser, email } = JSON.parse(storedAuth);
        this.idUser = idUser;
        this.email = email;
        return;
      }

      // Если в sessionStorage нет — пытаемся раскодировать токен
      const decoded = decodeJwtToken();
      if (decoded) {
        this.idUser = decoded.idUser;
        this.email = decoded.email;

        // Сохраняем в сессию
        this.saveToSessionStorage();
      }
    },

    // Сохранение в сессионное хранилище
    saveToSessionStorage() {
      const authData = {
        idUser: this.idUser,
        email: this.email,
      };
      sessionStorage.setItem('auth', JSON.stringify(authData));
    },

    // Удаление из сессионного хранилища (например, при логауте)
    clearSessionStorage() {
      sessionStorage.removeItem('auth');
    },
  },
});

// src/store/index.js

export const useThemeStore = defineStore('theme', {
  state: () => ({
    idThemeSet: null,
    theme: null,
    importanceThemes: [], // Массив с цветами важности
  }),
  actions: {
    async loadTheme(themeId) {
      const authStore = useAuthStore();
      if (!authStore.idUser) {
        console.error('Пользователь не авторизован');
        return;
      }
      try {
        const themeSet = await fetchThemeSet(authStore.idUser, themeId);
        
        this.theme = themeSet.theme;
        this.importanceThemes = themeSet.importanceThemes;

        this.applyTheme();
        this.applyImportanceStyles(); // применяем цвета важности

        sessionStorage.setItem('theme', JSON.stringify(this.theme));
        sessionStorage.setItem('importanceThemes', JSON.stringify(this.importanceThemes));

        this.idThemeSet = themeSet.idThemeSet;
      } catch (error) {
        console.error('Не удалось загрузить тему:', error);
      }
    },

    loadThemeFromStorage() {
      const storedTheme = sessionStorage.getItem('theme');
      const storedImportanceThemes = sessionStorage.getItem('importanceThemes');

      if (storedTheme) {
        this.theme = JSON.parse(storedTheme);
        this.applyTheme();
      }

      if (storedImportanceThemes) {
        this.importanceThemes = JSON.parse(storedImportanceThemes);
        this.applyImportanceStyles();
      }
    },

    applyTheme() {
      if (!this.theme) return;

      const root = document.documentElement;
      const theme = this.theme;

      root.style.setProperty('--color-background', theme.backgroundColor);
      root.style.setProperty('--color-text', theme.textColor);
      root.style.setProperty('--color-heading', theme.textColor);
      root.style.setProperty('--color-border', theme.borderColor);
      root.style.setProperty('--color-background-soft', theme.cardBackground);
      root.style.setProperty('--color-button', theme.buttonColor);
      root.style.setProperty('--color-button-text', theme.buttonTextColor);
      root.style.setProperty('--color-accent', theme.accentColor);
      root.style.setProperty('--color-primary', theme.primaryColor);
      root.style.setProperty('--color-secondary', theme.secondaryColor);
      root.style.setProperty('--color-shadow', theme.shadowColor);
    },

    applyImportanceStyles() {
      const root = document.documentElement;

      this.importanceThemes.forEach((importance, index) => {
        const prefix = `--importance-${importance.idImportance}`;
        root.style.setProperty(`${prefix}-bg`, importance.backgroundColor);
        root.style.setProperty(`${prefix}-text`, importance.textColor);
      });
    },
  },
});