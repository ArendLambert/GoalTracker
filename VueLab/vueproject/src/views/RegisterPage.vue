<template>
  <div class="login-container">
    <h1 class="title">Создание аккаунта</h1>
    <form @submit.prevent="signUp" class="login-form">
      <input
        v-model="emailInput"
        id="emailInput"
        type="email"
        placeholder="Введите email (mail.ru)"
        required
      />
      <input
        v-model="passwordInput"
        id="passwordInput"
        type="password"
        placeholder="Введите пароль"
        required
      />
      <button type="submit" :disabled="isLoading">
        {{ isLoading ? "Загрузка..." : "Создать аккаунт" }}
      </button>
    </form>
    <p class="switch-auth">
      Уже есть аккаунт?
      <router-link to="/loginpage">Войти</router-link>
    </p>
  </div>
</template>

<script>
import { useAuthStore, useThemeStore } from "@/store";
import { decodeJwtToken } from "@/utils/jwt";
import {
  fetchCreateUser,
  fetchExistUserByEmail,
  fetchUpdateUser,
} from "@/services/api";
import router from "../router/script";
import { toast } from "vue3-toastify";

export default {
  name: "RegisterPage",
  data() {
    return {
      emailInput: "",
      passwordInput: "",
      isLoading: false,
    };
  },
  mounted() {
    const decoded = decodeJwtToken();
    if (decoded) {
      router.push({ path: "/goals" });
    }
  },
  methods: {
    async signUp() {
      this.isLoading = true;
      try {
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(this.emailInput)) {
          alert("Введите корректный email");
          return;
        }
        const existUser = await fetchExistUserByEmail(this.emailInput);
        if (existUser) {
          this.toast.error("Пользователь с таким email уже существует");
          return;
        }
        await fetchCreateUser(this.emailInput, this.passwordInput);
        const decoded = decodeJwtToken();
        if (decoded) {
          const authStore = useAuthStore();
          authStore.idUser = decoded.idUser;
          authStore.email = decoded.email;
          authStore.saveToSessionStorage();
          const themeStore = useThemeStore();
          await themeStore.loadTheme("5EC6627F-1F1B-47E6-8EBD-367BC345F702");
          router.push({ path: "/goals" });
        } else {
          toast.error("Ошибка: токен не получен");
        }
      } catch (error) {
        console.error(
          "Ошибка регистрации:",
          error.response?.data || error.message,
        );
        toast.error("Ошибка: токен не получен");
      } finally {
        this.isLoading = false;
      }
    },
  },
  setup() {
    return { toast };
  },
};
</script>

<style scoped>
.login-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 2rem;
  background-color: var(--color-background);
  color: var(--color-text);
}

.title {
  font-size: 2rem;
  color: var(--color-heading);
  margin-bottom: 1.5rem;
  text-align: center;
}

.login-form {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
  width: 100%;
  max-width: 500px;
  background-color: var(--color-background-soft);
  padding: 2rem;
  border-radius: 8px;
  border: 1px solid var(--color-border);
  box-shadow: 0 4px 12px var(--color-shadow);
}

input {
  width: 100%;
  padding: 12px;
  border: 1px solid var(--color-border);
  border-radius: 6px;
  background-color: var(--color-background);
  color: var(--color-text);
  font-size: 1rem;
  transition: border-color 0.3s ease;
}

input:focus {
  outline: none;
  border-color: var(--color-border-hover);
  box-shadow: 0 0 5px var(--color-shadow);
}

input::placeholder {
  color: var(--color-text);
  opacity: 0.6;
}

button {
  width: 100%;
  padding: 12px;
  background-color: var(--color-button);
  color: var(--color-button-text);
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

button:hover {
  background-color: var(--color-accent);
}

button:focus {
  outline: none;
  box-shadow: 0 0 5px var(--color-shadow);
}

button:disabled {
  background-color: var(--color-border);
  cursor: not-allowed;
}

@media (max-width: 600px) {
  .login-form {
    max-width: 90%;
    padding: 1.5rem;
  }
  .title {
    font-size: 1.5rem;
  }
}

@media (min-width: 1024px) {
  .login-container {
    grid-column: span 2;
  }
}

.switch-auth {
  margin-top: 1rem;
  font-size: 0.95rem;
  text-align: center;
}

.switch-auth a {
  color: var(--color-accent);
  text-decoration: none;
  font-weight: 500;
  margin-left: 4px;
  transition: color 0.3s ease;
}

.switch-auth a:hover {
  color: var(--color-accent-hover, #a442d1);
}
</style>
