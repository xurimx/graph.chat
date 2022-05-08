import {createRouter, createWebHistory} from "vue-router";
import Home from '../views/Home.vue';
import Authenticate from '../views/Authenticate.vue';
import {USER_TOKEN, USER_ROLE} from "../constants/keys";

const routes = [
    {path: '/', name: 'Home', component: Home, meta: {authorize: 'User'}},
    {path: '/authenticate', name: 'Authenticate', component: Authenticate},
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
});

router.beforeEach((to, from, next) => {
    const { authorize } = to.meta;
    const token = localStorage.getItem(USER_TOKEN);
    const role = localStorage.getItem(USER_ROLE);

    if (authorize) {
        if (!token) {
            return next({ path: '/authenticate', query: { returnUrl: to.path } });
        }
        if (authorize !== role) {
            return next({ path: '/authenticate' });
        }
    }
    next();
});

export default router;