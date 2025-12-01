<template>
  <div class="container">
    <header class="header">
      <div class="brand"> 
        <img src="@/assets/owl_logo.png" alt="three owls logo" />
        <h1>THE THREE OWLS BOOKSTORE .</h1>
      </div>

      <nav class="nav" v-if="!isAuthPage">
        <router-link class="item" to="/">🏠 Home</router-link>
        <router-link class="item" to="/">📚 Books</router-link>
        <router-link class="item" to="/">🏷️ Discounts</router-link>
        <div class="cart">🛒 130 €</div>
      </nav>
    </header>

    <router-view />
  </div>
</template>

<script>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

export default {
  name: 'App',
  setup() {
    const route = useRoute();
    
    const isAuthPage = computed(() => {
      return route.name === 'Login' || route.name === 'Register';
    });

    return {
      isAuthPage
    };
  }
}
</script>

<style scoped>
/* keep small spacing only; global styles are in styles.css */
.brand{
  display:flex;
  align-items:center;
  gap:12px;
}
.brand img {
  width: 56px;
  height: auto;
  display: inline-block;
}
.brand h1 {
  font-weight: 300;
  letter-spacing: 8px;
  font-size: 1.2rem;
  margin: 0;
}

.header{ padding: 8px 0; }
.header .nav{ margin-left: 18px; }
.header .brand{ flex: 1 1 auto; }
.header .cart{ margin-left: auto; }
.header .cart{
  min-width: 80px;
  padding: 10px 18px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border-radius: 18px;
  background: #9fe6df;
  font-weight: 700;
  color: #083533;
}
</style>
