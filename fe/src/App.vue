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
                <router-link class="item" to="/stores">🏪 Stores</router-link>
                <router-link class="item" to="/">🏷️ Discounts</router-link>
                <div class="lang-select">
                    <button @click="openLang = !openLang">
                        🌍 Language
                    </button>

                    <div v-if="openLang" class="lang-menu">
                        <div @click="setLang('en')">🇬🇧 English</div>
                        <div @click="setLang('sl')">🇸🇮 Slovenščina</div>
                        <div @click="setLang('de')">🇩🇪 Deutsch</div>
                    </div>

                    <!-- hidden Google element -->
                    <div id="google_translate_element" style="display:none"></div>
                </div>

                <router-link to="/cart" class="cart">🛒 {{ cartTotal.toFixed(2) }} €</router-link>
            </nav>
        </header>

        <router-view />
    </div>
</template>

<script>
    import { computed, ref } from 'vue';
    import { useRoute } from 'vue-router';
    import cartService from './services/cart-service';

    export default {
        name: 'App',
        setup() {
            const route = useRoute();
            const openLang = ref(false);

            const isAuthPage = computed(() => {
                return route.name === 'Login' || route.name === 'Register';
            });

            const cartTotal = computed(() => {
                return cartService.getTotal();
            });

            const setLang = (lang) => {
                const select = document.querySelector('select.goog-te-combo');
                if (!select) {
                    console.warn('Google Translate not loaded yet');
                    return;
                }

                select.value = lang;
                select.dispatchEvent(new Event('change'));
                openLang.value = false;
            };

            return {
                isAuthPage,
                cartTotal,
                openLang,
                setLang
            };
        }
    }
</script>



<style scoped>
    /* keep small spacing only; global styles are in styles.css */
    .brand {
        display: flex;
        align-items: center;
        gap: 12px;
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

    .header {
        padding: 8px 0;
    }

        .header .nav {
            margin-left: 18px;
        }

        .header .brand {
            flex: 1 1 auto;
        }

        .header .cart {
            margin-left: auto;
        }

        .header .cart {
            min-width: 80px;
            padding: 10px 18px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            border-radius: 18px;
            background: #9fe6df;
            font-weight: 700;
            color: #083533;
            text-decoration: none;
            cursor: pointer;
            transition: background-color 0.2s;
        }

            .header .cart:hover {
                background: #8dd5cd;
            }

    .lang-select {
        position: relative;
    }

        .lang-select button {
            padding: 6px 12px;
            border-radius: 8px;
            border: 1px solid #ddd;
            background: white;
            cursor: pointer;
        }

    .lang-menu {
        position: absolute;
        top: 110%;
        right: 0;
        background: white;
        border-radius: 8px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    }

        .lang-menu div {
            padding: 8px 12px;
            cursor: pointer;
        }

            .lang-menu div:hover {
                background: #f0f0f0;
            }
</style>
