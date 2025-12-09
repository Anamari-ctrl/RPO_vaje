<template>
    <nav class="nav">
        <div class="search-bar-1">
            <input v-model="searchQuery"
                   type="text"
                   class="search-bar"
                   placeholder="🔍 Search books..." />
        </div>
        <div class="nav-1">
            <router-link class="item" to="/profile">👤 Profile</router-link>
            <router-link class="item" to="/">❤︎ Wishlist</router-link>
        </div>
    </nav>
    <div class="content">
        <aside class="sidebar">
            <div class="filters">
                <h3>Genres</h3>
                <label class="filter-pill"><input type="checkbox" v-model="filters.genres.cs" /><span>Computer Science</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.genres.fiction" /><span>Fiction</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.genres.nonfiction" /><span>Non-fiction</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.genres.history" /><span>History</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.genres.scifi" /><span>Sci-Fi</span></label>

                <h3>Format</h3>
                <label class="filter-pill"><input type="checkbox" v-model="filters.format.book" /><span>Hardcover</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.format.ebook" /><span>eBook</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.format.audio" /><span>Audio Book</span></label>

                <h3>Availability</h3>
                <label class="filter-pill"><input type="checkbox" v-model="filters.availability.new" /><span>New Arrivals</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.availability.sale" /><span>On Sale</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.availability.inStock" /><span>In Stock</span></label>

                <h3>Language</h3>
                <label class="filter-pill"><input type="checkbox" v-model="filters.lang.en" /><span>English</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.lang.de" /><span>German</span></label>
                <label class="filter-pill"><input type="checkbox" v-model="filters.lang.sl" /><span>Slovene</span></label>

            </div>
        </aside>

        <main class="main">
            <h2 class="section-title">Books</h2>
            <div class="grid">
                <div class="card" v-for="(book, idx) in books" :key="idx">
                    <img :src="book.image" :alt="book.title" />
                    <div class="price">{{ book.price }} €</div>
                    <button class="buy">BUY</button>
                    <div class="description">{{ book.description }}</div>
                    <div class="stock">Stock: {{ book.stock }}</div>
                </div>
            </div>
        </main>
    </div>
</template>

<script>
    export default {
        name: 'HomeView',
        data() {
            return {
                books: [
                    { title: 'Essential Computer Science', price: 40, image: require('@/assets/book1.jpg'),stock: 2 , description: 'A Programmer’s Guide to Foundational Concepts 1st ed. Edition' },
                    { title: 'Intro to Computer Science', price: 35, image: require('@/assets/book2.svg'), stock: 4, description: 'A friendly introduction for curious middle and high school students on their way to learn the basic fundamentals' },
                    { title: 'Algorithms and Data', price: 57, image: require('@/assets/book3.svg'), stock: 8, description: 'Advanced Algorithms and Data Structures' },
                    { title: 'Essential Computer Science', price: 22, image: require('@/assets/book4.svg'), stock: 0, description: 'A Programmer’s Guide to Foundational Concepts 1st ed. Edition' },
                    { title: 'Programming Essentials', price: 29, image: require('@/assets/book5.svg'), stock: 1, description: 'Provides students with a deep, working understanding of the essential concepts of programming languages, completely revised, with significant new material.' },
                    { title: 'Foundations of CS', price: 48, image: require('@/assets/book6.svg'), stock: 5, description: 'Foundations-of-CS is an open, community-driven primer covering the essential building blocks of computer science.' },
                    { title: 'Algorithms and Data', price: 57, image: require('@/assets/book3.svg'), stock: 2, description: 'Advanced Algorithms and Data Structures' },
                    { title: 'Essential Computer Science', price: 22, image: require('@/assets/book4.svg'), stock: 3, description: 'A Programmer’s Guide to Foundational Concepts 1st ed. Edition' },
                    { title: 'Programming Essentials', price: 29, image: require('@/assets/book5.svg'), stock: 5, description: 'Provides students with a deep, working understanding of the essential concepts of programming languages, completely revised, with significant new material.' },
                    { title: 'Foundations of CS', price: 48, image: require('@/assets/book6.svg'), stock: 0, description: 'Foundations-of-CS is an open, community-driven primer covering the essential building blocks of computer science.' }
                ],
                filters: {
                    genres: { cs: true, fiction: false, nonfiction: false, history: false, scifi: false },
                    format: { book: true, ebook: false, audio: false },
                    availability: { new: false, sale: false, inStock: true },
                    lang: { en: true, de: false, sl: false }
                }
            }
        }
    }
</script>

<style scoped>
    /* Layout: sidebar + main */
    .content {
        display: grid;
        grid-template-columns: 280px 1fr;
        gap: 0px;
        align-items: start;
    }

    .sidebar {
        background: var(--panel);
        border-right: 1px solid var(--muted);
        padding: 18px;
    }

    .filters {
        display: flex;
        flex-direction: column;
        gap: 18px;
    }

        .filters h3 {
            margin: 0 0 8px 0;
            font-size: 1rem;
        }

    .filter-pill {
        display: flex;
        align-items: center;
        gap: 10px;
        padding: 6px 6px;
        cursor: pointer;
    }

        .filter-pill input {
            width: 16px;
            height: 16px;
            margin-right: 6px;
        }

        .filter-pill span {
            font-size: 0.95rem;
        }

    .color-box {
        width: 14px;
        height: 14px;
        border-radius: 3px;
    }

    /* Main area */
    .main {
        background: transparent;
    }

    .grid {
        display: flex;
        flex-wrap: wrap;
        gap: 20px;
        align-items: flex-start;
        justify-content: center;
    }

    .card {
        width: 240px;
        background: white;
        box-shadow: 0 1px 0 rgba(0,0,0,0.02);
        padding: 14px;
        text-align: center;
        border-radius: 12px;
        overflow: hidden;
    }
        .card:hover {
            transform: translateY(-6px);
            overflow: visible;
        }

        .card img {
            width: 100%;
            height: 300px;
            object-fit: cover;
            margin-bottom: 12px;
            border-radius: 6px;
        }

    .price {
        display: inline-block;
        background: rgba(255,109,47,0.08);
        color: var(--accent);
        font-weight: 700;
        margin-bottom: 8px;
        font-size: 1.1rem;
        padding: 6px 14px;
        border-radius: 999px;
    }

    .buy {
        border-radius: 18px;
        border: 2px solid #cfcfcf;
        background: transparent;
        padding: 6px 14px;
        color: #777;
        cursor: pointer;
    }



    .filters > div + h3 {
        margin-top: 12px;
    }

    @media (max-width: 900px) {
        .content {
            grid-template-columns: 1fr;
        }

        .sidebar {
            order: 2
        }

        .main {
            order: 1
        }

        .card img {
            height: 160px
        }
    }

    .nav {
        display: flex;
        align-items: center;
        gap: 20px;
        padding: 8px 16px; /* thinner nav */
        background: var(--panel);
        border-bottom: 1px solid var(--muted);
        width: 100%;
    }

    .nav-1 {
        text-align: right;
        width: 50%;
    }

    .item {
        text-decoration: none;
        color: var(--text);
        font-weight: 500;
    }

    .search-bar {
        margin-left: auto;
        padding: 6px 12px;
        border-radius: 6px;
        border: 1px solid #ccc;
        font-size: 0.95rem;
        width: 100%;
    }

    .search-bar-1 {
        padding: 6px 12px;
        width: 50%;
    }


    .description {
        max-height: 40px; 
        overflow: hidden;
        font-size: 0.9rem;
        color: #444;
    }
    .card:hover .description {
        overflow: visible;
        max-height: 400px;
    }

</style>
