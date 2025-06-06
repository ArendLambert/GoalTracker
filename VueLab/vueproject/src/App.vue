<script>
import { decodeJwtToken } from "@/utils/jwt";
import router from "./router/script";
import { toast } from "vue3-toastify";
import { useAuthStore, useThemeStore } from "@/store";
export default {
  data() {
    return {
      inputDesc: "",
      importance: "",
      deals: [],
      date: null,
    };
  },
  mounted() {
    // const decoded = decodeJwtToken();
    //       if (!decoded){
    //         router.push({ path: '/registerpage' });
    //       }
    const themeStore = useThemeStore();
    themeStore.loadThemeFromStorage();
    // if(['/'].includes(this.$route.path)){
    //   router.push({ path: '/loginpage' });
    // }
  },
  setup() {
    const authStore = useAuthStore();
    authStore.initializeAuth();
    return { toast };
  },
  methods: {
    AddDeal() {
      this.toast.success("ТЕСТ");
      this.deals.push({
        id: this.deals.length + 1,
        desc: this.inputDesc,
        status: this.importance,
        date: this.date,
      });
    },
    RemoveDeal(id) {
      this.deals.splice(
        this.deals.findIndex((item) => item.id === id),
        1,
      );
    },
  },
  computed: {
    showRouterView() {
      return !["/loginpage", "/registerpage", "/goals", "/themestore", "/wheel"].includes(
        this.$route.path,
      );
    },
  },
};
</script>

<template>
  <router-view />
  <div v-if="showRouterView">
    <p>ЗАДАЧИ ДЛЯ ЕГОРА</p>
    <div style="display: flex; flex-direction: column">
      <div style="display: flex; flex-direction: row">
        <input
          v-model="inputDesc"
          id="inputDesc"
          style="color: antiquewhite"
          type="text"
          placeholder="Введите текст задачи"
        />
        <v-combobox
          label="Степень важности задачки"
          :items="[
            'Важно до безумия',
            'Важно',
            'Терпица слюбица',
            'На потом',
            'Забей ваще',
            'НЕ ДЕЛАТЬ',
          ]"
          v-model="importance"
          style="min-width: 200px"
        >
        </v-combobox>

        <DatePicker v-model="date" dateFormat="dd/mm/yy" />
        <v-date-input
          label="Дата постановки"
          v-model="date"
          style="min-width: 200px"
        ></v-date-input>
      </div>
      <div>
        <button @click="AddDeal">ДОБАВИТЬ ДЕЛО</button>
      </div>
    </div>
    <div>
      <li v-for="item in this.deals" :key="item.id">
        <span>Описание: {{ item.desc }}</span>
        <p></p>
        <span>Степень важности: {{ item.status }}</span>
        <p></p>
        <span>Врямя: {{ item.date.toLocaleDateString("ru-RU") }}</span>
        <p></p>
        <button @click="RemoveDeal(item.id)">УДАЛИТЬ</button>
      </li>
    </div>
  </div>
</template>

<style scoped>
button {
  margin-top: 10px;
  padding: 10px 20px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}
</style>
