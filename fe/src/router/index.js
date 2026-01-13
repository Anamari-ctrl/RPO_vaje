import { createRouter, createWebHistory } from 'vue-router'

import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import ForgotPasswordView from '../views/ForgotPasswordView.vue'
import ResetPasswordView from '../views/ResetPasswordView.vue'
import ProfileView from '../views/ProfileView.vue'
import CartView from '../views/CartView.vue'
import StoresView from '../views/StoresView.vue'
import BookDetailView from '../views/BookDetailView.vue'

import accountService from '../services/account-service'

const routes = [
    { path: '/', name: 'Home', component: () => import('../views/IndexView.vue') },
    { path: '/books', name: 'Books', component: () => import('../views/HomeView.vue') },

    { path: '/login', name: 'Login', component: LoginView },
    { path: '/register', name: 'Register', component: RegisterView },
    { path: '/forgot-password', name: 'ForgotPassword', component: ForgotPasswordView },
    { path: '/reset-password', name: 'ResetPassword', component: ResetPasswordView },


    { path: '/profile', name: 'Profile', component: ProfileView, meta: { requiresAuth: true } },
    { path: '/cart', name: 'Cart', component: CartView },
    { path: '/stores', name: 'Stores', component: StoresView },

    { path: '/books/:id', name: 'BookDetail', component: BookDetailView, props: true },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach((to, from, next) => {
    const isLoggedIn = accountService.hasToken();

    //  Protected routes
    if (to.meta.requiresAuth && !isLoggedIn) {
        next({
            name: 'Login',
            query: { redirect: to.fullPath } 
        });
        return;
    }

    if (isLoggedIn && (to.name === 'Login' || to.name === 'Register')) {
        next({ name: 'Home' });
        return;
    }

    next();
});


export default router
