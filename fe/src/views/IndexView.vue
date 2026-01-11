<template>
    <nav class="nav">
        <div class="nav-1">
            <div class="tip-wrap">
                <router-link class="item" to="/profile" aria-describedby="tt-profile">👤 Profile</router-link>
                <span id="tt-profile" role="tooltip" class="tooltip">View and edit your profile</span>
            </div>

            <div class="tip-wrap">
                <router-link class="item" to="/" aria-describedby="tt-wishlist">❤︎ Wishlist</router-link>
                <span id="tt-wishlist" role="tooltip" class="tooltip">See your saved books</span>
            </div>
        </div>
    </nav>

    <div class="home">
        <div class="hero">
            <img src="@/assets/owl_logo.png"
                 alt="Three Owls Bookstore logo"
                 class="logo" />

            <h1>The Three Owls Bookstore</h1>

            <p class="subtitle">
                A modern web application for browsing and buying books online.
            </p>

            <p class="description">
                This application was developed as part of the RPO course.
                It demonstrates client-server architecture, REST API usage,
                filtering, pagination, and accessible UI design.
            </p>

            <div class="authors">
                <h3>Project authors</h3>
                <ul>
                    <li>
                        Anamari Orehar –
                        <a href="mailto:anamari.orehar@gmail.com">anamari.orehar@gmail.com</a>
                    </li>
                    <li>
                        Miha Potočnik –
                        <a href="mailto:mihapot@gmail.com">mihapot@gmail.com</a>
                    </li>
                    <li>
                        Miša Rožman Atelšek –
                        <a href="mailto:misarozmana@gmail.com">misarozmana@gmail.com</a>
                    </li>


                </ul>
            </div>

        </div>
    </div>
</template>

<script>
export default {
  name: 'IndexView'
};
</script>

<style scoped>
    .home {
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 60px 20px;
    }

    .hero {
        max-width: 700px;
        text-align: center;
        background: white;
        padding: 40px;
        border-radius: 20px;
        box-shadow: 0 10px 40px rgba(0,0,0,0.08);
    }

    .logo {
        width: 140px;
        margin-bottom: 20px;
    }

    h1 {
        font-weight: 300;
        letter-spacing: 6px;
        margin-bottom: 12px;
    }

    .subtitle {
        font-size: 1.1rem;
        color: #666;
        margin-bottom: 20px;
    }

    .description {
        font-size: 0.95rem;
        color: #555;
        margin-bottom: 30px;
        line-height: 1.6;
    }

    .authors {
        margin-bottom: 30px;
    }

        .authors h3 {
            margin-bottom: 10px;
            font-weight: 500;
        }

        .authors ul {
            list-style: none;
            padding: 0;
            margin: 0;
        }

        .authors li {
            font-size: 0.95rem;
            color: #444;
        }

        .authors a {
            color: var(--accent);
            text-decoration: none;
            font-weight: 500;
        }

            .authors a:hover {
                text-decoration: underline;
            }


    .actions {
        display: flex;
        justify-content: center;
        gap: 16px;
    }

    .btn {
        padding: 12px 22px;
        border-radius: 999px;
        text-decoration: none;
        font-weight: 600;
        transition: transform 0.15s ease, box-shadow 0.15s ease;
    }

        .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 16px rgba(0,0,0,0.15);
        }

    .primary {
        background: var(--accent);
        color: white;
    }

    .secondary {
        background: #f0f0f0;
        color: #333;
    }
    .item {
        text-decoration: none;
        color: var(--text);
        font-weight: 500;
        padding: 6px 10px;
        border-radius: 6px;
        transition: background 0.2s;
    }

        .item:hover {
            background: rgba(0,0,0,0.05);
        }

    .nav {
        display: flex;
        align-items: center;
        padding: 8px 16px;
        background: var(--panel);
        border-bottom: 1px solid var(--muted);
        width: 100%;
    }

    /* push this container to the RIGHT */
    .nav-1 {
        margin-left: auto;
        display: flex;
        gap: 20px;
        align-items: center;
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
        z-index: 1000;
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

    .tip-wrap:hover > .tooltip,
    .tip-wrap:focus-within > .tooltip {
        opacity: 1;
        visibility: visible;
        transform: translateX(-50%) translateY(0);
    }

    /* Reduced motion */
    @media (prefers-reduced-motion: reduce) {
        .tooltip {
            transition: none;
        }
    }

</style>
