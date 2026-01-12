<template>
    <div class="container">
        <header class="header">
            <div class="brand">
                <img src="@/assets/owl_logo.png" alt="three owls logo" />
                <h1>THE THREE OWLS BOOKSTORE .</h1>
            </div>

            <!-- MOBILE: hamburger + cart -->
            <div v-if="!isAuthPage" class="mobile-actions">
                <button class="burger"
                        type="button"
                        @click="toggleMobileNav"
                        :aria-expanded="isMobileNavOpen ? 'true' : 'false'"
                        aria-label="Toggle navigation">
                    ☰
                </button>

                <router-link to="/cart" class="cart mobile-cart">
                    🛒 {{ cartTotal.toFixed(2) }} €
                </router-link>
            </div>

            <!-- NAV (desktop + mobile dropdown) -->
            <nav v-if="!isAuthPage"
                 class="nav"
                 :class="{ open: isMobileNavOpen }">
                <div class="tip-wrap">
                    <router-link class="item" to="/" aria-describedby="tt-home" @click="closeMobileNav">
                        🏠 Home
                    </router-link>
                    <span id="tt-home" role="tooltip" class="tooltip">Go to the homepage</span>
                </div>

                <div class="tip-wrap">
                    <router-link class="item" to="/books" aria-describedby="tt-books" @click="closeMobileNav">
                        📚 Books
                    </router-link>
                    <span id="tt-books" role="tooltip" class="tooltip">Browse all books</span>
                </div>

                <div class="tip-wrap">
                    <router-link class="item" to="/stores" aria-describedby="tt-stores" @click="closeMobileNav">
                        🏪 Stores
                    </router-link>
                    <span id="tt-stores" role="tooltip" class="tooltip">Find our store locations</span>
                </div>

                <div class="tip-wrap">
                    <router-link class="item" to="/" aria-describedby="tt-discounts" @click="closeMobileNav">
                        🏷️ Discounts
                    </router-link>
                    <span id="tt-discounts" role="tooltip" class="tooltip">See current deals</span>
                </div>

                <div class="lang-select tip-wrap">
                    <button type="button"
                            @click="openLang = !openLang"
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

                <!-- DESKTOP cart -->
                <div class="tip-wrap cart-wrap desktop-cart">
                    <router-link to="/cart" class="cart" aria-describedby="tt-cart" @click="closeMobileNav">
                        🛒 {{ cartTotal.toFixed(2) }} €
                    </router-link>
                    <span id="tt-cart" role="tooltip" class="tooltip">Open cart</span>
                </div>
            </nav>
        </header>

        <router-view />
    </div>
</template>

<script>
    import { computed, ref, watch } from "vue";
    import { useRoute } from "vue-router";
    import cartService from "./services/cart-service";

    export default {
        name: "App",
        setup() {
            const route = useRoute();
            const openLang = ref(false);

            const isMobileNavOpen = ref(false);

            const isAuthPage = computed(() => {
                return route.name === "Login" || route.name === "Register";
            });

            const cartTotal = computed(() => cartService.getTotal());

            const closeMobileNav = () => {
                isMobileNavOpen.value = false;
                openLang.value = false;
            };

            const toggleMobileNav = () => {
                isMobileNavOpen.value = !isMobileNavOpen.value;
                if (!isMobileNavOpen.value) openLang.value = false;
            };

            // ko zamenjaš stran, zapri mobile meni
            watch(
                () => route.fullPath,
                () => {
                    closeMobileNav();
                }
            );

            const setLang = (lang) => {
                const select = document.querySelector("select.goog-te-combo");
                if (!select) {
                    console.warn("Google Translate not loaded yet");
                    return;
                }

                select.value = lang;
                select.dispatchEvent(new Event("change"));
                openLang.value = false;
                isMobileNavOpen.value = false;
            };

            return {
                isAuthPage,
                cartTotal,
                openLang,
                setLang,

                isMobileNavOpen,
                toggleMobileNav,
                closeMobileNav,
            };
        },
    };
</script>

<style scoped>
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
        display: flex;
        align-items: center;
        gap: 12px;
    }

        .header .brand {
            flex: 1 1 auto;
        }

    /* NAV */
    .nav {
        display: flex;
        align-items: center;
        gap: 14px;
        margin-left: 18px;
    }

    /* CART (shared style) */
    .cart {
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

        .cart:hover {
            background: #8dd5cd;
        }

    /* Language dropdown */
    .lang-select {
        position: relative;
        z-index: 2000;
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
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        z-index: 3000;
    }

        .lang-menu div {
            padding: 8px 12px;
            cursor: pointer;
        }

            .lang-menu div:hover {
                background: #f0f0f0;
            }

    /* Tooltip */
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

    .lang-select[aria-expanded="true"] .tooltip {
        opacity: 0 !important;
        visibility: hidden !important;
    }

    /* MOBILE NAVBAR */
    .mobile-actions {
        display: none;
        align-items: center;
        gap: 10px;
    }

    .burger {
        border: 1px solid #ddd;
        background: white;
        border-radius: 10px;
        padding: 8px 12px;
        cursor: pointer;
        font-size: 18px;
        line-height: 1;
    }

    .desktop-cart {
        display: inline-flex;
    }

    .mobile-cart {
        padding: 8px 12px;
        min-width: auto;
    }

    /* Responsive behavior */
    @media (max-width: 900px) {
        .mobile-actions {
            display: inline-flex;
        }

        .desktop-cart {
            display: none;
        }

        /* nav becomes dropdown panel */
        .nav {
            position: absolute;
            left: 0;
            right: 0;
            top: 72px; /* približno višina headerja */
            margin-left: 0;
            padding: 12px 14px;
            background: var(--panel);
            border-top: 1px solid var(--muted);
            border-bottom: 1px solid var(--muted);
            display: none;
            flex-direction: column;
            align-items: flex-start;
            gap: 12px;
            z-index: 5000;
        }

            .nav.open {
                display: flex;
            }

        .header {
            position: relative;
        }
    }

    @media (prefers-reduced-motion: reduce) {
        .tooltip {
            transition: none;
        }
    }
</style>
