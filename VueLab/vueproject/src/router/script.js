import HomeView from '../views/HomeView.vue';
import NewWindowView from '../views/NewWindowView.vue';
import Register from '../views/RegisterPage.vue';
import Login from '../views/LoginPage.vue';
import Main from '../views/MainPage.vue';
import { createRouter, createWebHistory } from 'vue-router';

const routes = [
    {
        path: '/',
        component: HomeView,
    },
    {
        path: '/new-window',
        component: NewWindowView,
    },
    {path: '/registerpage',
        component: Register,
        name: 'Register',
    },
    {path: '/loginpage',
        component: Login,
        name: 'Login',
    },
    {path: '/goals',
        component: Main,
        name: 'Main',
    },
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
});

export default router;