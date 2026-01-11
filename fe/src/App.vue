<template>
    <div class="container">
        <header class="header">
            <div class="brand">
                <img src="@/assets/owl_logo.png" alt="three owls logo" />
                <h1>THE THREE OWLS BOOKSTORE .</h1>
            </div>

            <nav class="nav" v-if="!isAuthPage">
                <div class="tip-wrap">
                    <router-link class="item" to="/" aria-describedby="tt-home">🏠 Home</router-link>
                    <span id="tt-home" role="tooltip" class="tooltip">Go to the homepage</span>
                </div>

                <div class="tip-wrap">
                    <router-link class="item" to="/books" aria-describedby="tt-books">📚 Books</router-link>
                    <span id="tt-books" role="tooltip" class="tooltip">Browse all books</span>
                </div>

                <div class="tip-wrap">
                    <router-link class="item" to="/stores" aria-describedby="tt-stores">🏪 Stores</router-link>
                    <span id="tt-stores" role="tooltip" class="tooltip">Find our store locations</span>
                </div>

                <div class="tip-wrap">
                    <router-link class="item" to="/" aria-describedby="tt-discounts">🏷️ Discounts</router-link>
                    <span id="tt-discounts" role="tooltip" class="tooltip">See current deals</span>
                </div>

                <div class="lang-select tip-wrap">
                    <button @click="openLang = !openLang"
                            aria-describedby="tt-lang"
                            :aria-expanded="openLang ? 'true' : 'false'">
                        🌍 Language
                    </button>
                    <span id="tt-lang" role="tooltip" class="tooltip">Change site language</span>

                    <div v-if="openLang" class="lang-menu" role="menu">
                        <div @click="setLang('en')" role="menuitem" tabindex="0">🇬🇧 English</div>
                        <div @click="setLang('sl')" role="menuitem" tabindex="0">🇸🇮 Slovenščina</div>
                        <div @click="setLang('de')" role="menuitem" tabindex="0">🇩🇪 Deutsch</div>
                    </div>

                    <!-- hidden Google element -->
                    <div id="google_translate_element" style="display:none"></div>
                </div>

                <div class="tip-wrap cart-wrap">
                    <router-link to="/cart" class="cart" aria-describedby="tt-cart">
                        🛒 {{ cartTotal.toFixed(2) }} €
                    </router-link>
                    <span id="tt-cart" role="tooltip" class="tooltip">Open cart</span>
                </div>
            </nav>

        </header>

        <router-view v-slot="{ Component, route }">
            <transition name="page" mode="out-in">
                <component :is="Component" :key="route.fullPath" />
            </transition>
        </router-view>
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

    .page-enter-active, .page-leave-active {
        transition: opacity 180ms ease, transform 180ms ease;
    }

    .page-enter-from, .page-leave-to {
        opacity: 0;
        transform: translateY(6px);
    }

    .tip-wrap {
        position: relative;
        display: inline-flex;
        align-items: center;
    }

    
    .tooltip {
        position: absolute;
        top: calc(100% + 8px);
        left: 50%;
        transform: translateX(-50%) translateY(-4px);
        padding: 6px 10px;
        border-radius: 10px;
        background: #111;
        color: #fff;
        font-size: 12px;
        line-height: 1.2;
        white-space: nowrap;
        opacity: 0;
        visibility: hidden;
        pointer-events: none;
        transition: opacity 120ms ease, transform 120ms ease, visibility 120ms ease;
        z-index: 999;
    }

        
        .tooltip::after {
            content: "";
            position: absolute;
            top: -6px;
            left: 50%;
            transform: translateX(-50%);
            border-width: 6px;
            border-style: solid;
            border-color: transparent transparent #111 transparent;
        }

    .tip-wrap:hover .tooltip,
    .tip-wrap:focus-within .tooltip {
        opacity: 1;
        visibility: visible;
        transform: translateX(-50%) translateY(0);
    }

    @media (prefers-reduced-motion: reduce) {
        .tooltip {
            transition: none;
        }

        .page-enter-active, .page-leave-active {
            transition: none;
        }
    }


</style>
