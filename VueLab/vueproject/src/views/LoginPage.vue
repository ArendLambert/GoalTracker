<template>
  <div class="login-container">
    <h1 class="title">Вход в аккаунт</h1>
    <form @submit.prevent="signIn" class="login-form">
      <input
        v-model="emailInput"
        type="email"
        placeholder="Введите email (mail.ru)"
        required
      />
      <input
        v-model="passwordInput"
        type="password"
        placeholder="Введите пароль"
        required
      />
      <button type="submit" :disabled="isLoading">
        {{ isLoading ? 'Загрузка...' : 'Войти' }}
      </button>
    </form>
    <p class="switch-auth">
        Нет аккаунта?
        <router-link to="/registerpage">Создать</router-link>
    </p>
  </div>
</template>

<script>
import { useAuthStore, useThemeStore } from '@/store';
import { decodeJwtToken } from '@/utils/jwt';
import { fetchLoginUser, fetchExistUserByEmail, fetchGetUserById } from '@/services/api';
import router from '../router/script';
import { toast } from 'vue3-toastify';

export default {
  name: 'LoginPage',
  data() {
    return {
      emailInput: '',
      passwordInput: '',
      isLoading: false,
    };
  },
  mounted() {
    const decoded = decodeJwtToken();
          if (decoded){
            router.push({ path: '/goals' });
          }
  },
  methods: {
    async signIn() {
      this.isLoading = true;
      try {
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(this.emailInput)) {
          toast.error('Введите корректный email');
          return;
        }
        const existUser = await fetchExistUserByEmail(this.emailInput);
        if (!existUser) {
          this.toast.error('Пользователь с таким email не существует');
          return;
        }
        console.log(this.emailInput, this.passwordInput)
        await fetchLoginUser(this.emailInput, this.passwordInput);

        const decoded = decodeJwtToken();
        if (decoded) {
          const authStore = useAuthStore();
          authStore.idUser = decoded.idUser;
          authStore.email = decoded.email;
          authStore.saveToSessionStorage();
          const themeStore = useThemeStore();
          const user = await fetchGetUserById(authStore.idUser);
          console.log(user);
          console.log(user.idThemeSet);
          if (user.idThemeSet) {
            await themeStore.loadTheme(user.idThemeSet);
          } else {
            await themeStore.loadTheme('5EC6627F-1F1B-47E6-8EBD-367BC345F702');
          }
          toast.success('Вы успешно вошли!');
          router.push({ path: '/goals' });
        } else {
          toast.error('Ошибка: токен не получен');
        }
      } catch (error) {
        console.error('Ошибка входа:', error.response?.data || error.message);
        console.log(error)
        toast.error('Ошибка входа. Проверьте email и пароль.');
      } finally {
        this.isLoading = false;
      }
    },
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
