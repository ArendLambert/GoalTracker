import "./assets/main.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router/script";
import { createVuetify } from "vuetify";
import "vuetify/styles";
import { VCombobox } from "vuetify/lib/components/index.mjs";
import { VDateInput } from "vuetify/labs/VDateInput";
import { createPinia } from "pinia";
import "vue3-toastify/dist/index.css";
import Vue3Toasity from "vue3-toastify";
import Antd from "ant-design-vue";
import "ant-design-vue/dist/reset.css";

function resolveGLobalComponents(instance) {
  instance.use(Antd);
}

const pinia = createPinia();
const vuetify = createVuetify({
  components: {
    VCombobox,
    VDateInput,
  },
});

const app = createApp(App).use(router).use(vuetify).use(pinia).use(Vue3Toasity);

resolveGLobalComponents(app);
app.use(Vue3Toasity, {
  // the Toast application is separate from the main application, so we need to call .use
  useHandler: resolveGLobalComponents,
  // other props...
});
app.mount("#app");
