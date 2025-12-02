import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import accountService from '../services/account-service'

const routes = [
  { path: '/', name: 'Home', component: Home },
    { path: '/login', name: 'Login', component: LoginView },
    { path: '/register', name: 'Register', component: RegisterView },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})


router.beforeEach((to, from, next) => {
  const isLoggedIn = accountService.hasToken();
  
  if (isLoggedIn && (to.name === 'Login'))  {
    next({ name: 'Home' });
  } else {
    next();
  }
});

export default router
