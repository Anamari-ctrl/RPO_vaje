<template>
    <nav class="nav">
        <div class="search-bar-1">
            <input v-model="searchQuery"
                   type="text"
                   class="search-bar"
                   placeholder="🔍 Search books..."
                   @input="handleSearch" />
        </div>
        <div class="nav-1">
            <router-link class="item" to="/profile">👤 Profile</router-link>
            <router-link class="item" to="/">❤︎ Wishlist</router-link>
        </div>
    </nav>
    <div class="content">
        <aside class="sidebar">
            <div class="filters">
                <h3>Sort By</h3>
                <select v-model="sortBy" @change="handleSortChange" class="sort-select">
                    <option value="title">Title (A-Z)</option>
                    <option value="price">Price (Low to High)</option>
                    <option value="createdAt">Newest</option>
                </select>

                <h3>Price Range</h3>
                <div class="price-filter">
                    <input v-model.number="filterValues.minPrice" type="number" placeholder="Min €" @input="handleFilterChange" class="price-input" />
                    <span class="price-separator">-</span>
                    <input v-model.number="filterValues.maxPrice" type="number" placeholder="Max €" @input="handleFilterChange" class="price-input" />
                </div>

                <h3>Availability</h3>
                <label class="filter-pill">
                    <input type="checkbox" v-model="filterValues.inStock" @change="handleFilterChange" />
                    <span>In Stock</span>
                </label>

                <h3>Brand</h3>
                <input v-model="filterValues.brand" type="text" placeholder="Brand" class="filter-input" @input="handleFilterChange" />

                <h3>Supplier</h3>
                <input v-model="filterValues.supplier" type="text" placeholder="Supplier" class="filter-input" @input="handleFilterChange" />

                <button @click="resetFilters" class="reset-btn">Reset Filters</button>
            </div>
        </aside>

        <main class="main">
            <div class="main-header">
                <h2 class="section-title">Books</h2>
                <div class="results-info" v-if="!loading">
                    Showing {{ (currentPage - 1) * pageSize + 1 }}-{{ Math.min(currentPage * pageSize, totalCount) }} of {{ totalCount }} books
                </div>
            </div>

            <div v-if="loading" class="loading">Loading books...</div>

            <div v-else-if="books.length === 0" class="no-results">
                No books found. Try adjusting your filters.
            </div>

            <div v-else class="grid">
                <div class="card clickable"
                     v-for="book in books"
                     :key="book.id"
                     @click="openBook(book.id)">

                    <img :src="book.image || 'https://via.placeholder.com/240x300?text=No+Image'" :alt="book.title" />
                    <div class="price">{{ book.price }} €</div>
                    <button 
                        class="buy" 
                        @click.stop="addToCart(book)"
                        :disabled="book.stock === 0">
                        {{ book.stock === 0 ? 'OUT OF STOCK' : 'ADD TO CART' }}
                    </button>
                    <div class="description">{{ book.title }}</div>
                    <div class="description">{{ book.shortDescription }}</div>
                    <div class="stock" :class="{ 'out-of-stock': book.stock === 0 }">
                        Stock: {{ book.stock }}
                    </div>
                </div>
            </div>

            <!-- Pagination -->
            <div v-if="totalPages > 1" class="pagination">
                <button 
                    @click="previousPage" 
                    :disabled="currentPage === 1"
                    class="btn-page">
                    ← Previous
                </button>
                
                <div class="page-numbers">
                    <button
                        v-for="page in paginationButtons"
                        :key="page"
                        @click="goToPage(page)"
                        :class="{ active: currentPage === page }"
                        class="btn-page-number">
                        {{ page }}
                    </button>
                </div>

                <button 
                    @click="nextPage" 
                    :disabled="currentPage === totalPages"
                    class="btn-page">
                    Next →
                </button>
            </div>
        </main>
    </div>
</template>

<script>
import booksService from '../services/books-service';
import cartService from '../services/cart-service';
import { useRouter } from 'vue-router';

export default {
    name: 'HomeView',
    setup() {
        const router = useRouter();

        const openBook = (bookId) => {
            router.push({ name: 'BookDetail', params: { id: bookId } });
        };

        const addToCart = (book) => {
            if (book.stock > 0) {
                cartService.addItem(book, 1);
                // Optional: Show a success message or toast notification
                alert(`Added "${book.title}" to cart!`);
            }
        };

        return {
            openBook,
            addToCart
        };
    },
    data() {
        return {
            books: [],
            searchQuery: '',
            currentPage: 1,
            pageSize: 12,
            totalCount: 0,
            totalPages: 0,
            loading: false,
            sortBy: 'title',
            filterValues: {
                minPrice: null,
                maxPrice: null,
                inStock: false,
                brand: '',
                supplier: ''
            }
        };
    },
    computed: {
        paginationButtons() {
            const buttons = [];
            const maxButtons = 5;
            
            let start = Math.max(1, this.currentPage - Math.floor(maxButtons / 2));
            let end = Math.min(this.totalPages, start + maxButtons - 1);
            
            if (end - start < maxButtons - 1) {
                start = Math.max(1, end - maxButtons + 1);
            }
            
            for (let i = start; i <= end; i++) {
                buttons.push(i);
            }
            
            return buttons;
        }
    },
    methods: {
        async fetchBooks() {
            this.loading = true;
            try {
                const result = await booksService.getBooks({
                    page: this.currentPage,
                    pageSize: this.pageSize,
                    sortBy: this.sortBy,
                    sortOrder: this.sortBy === 'price' ? 'asc' : 'asc',
                    filters: {
                        search: this.searchQuery,
                        minPrice: this.filterValues.minPrice,
                        maxPrice: this.filterValues.maxPrice,
                        inStock: this.filterValues.inStock ? 'true' : null,
                        brand: this.filterValues.brand,
                        supplier: this.filterValues.supplier
                    }
                });
                
                this.books = result.items || [];
                this.totalCount = result.totalCount || 0;
                this.currentPage = result.currentPage || 1;
                this.totalPages = result.totalPages || 1;
            } catch (error) {
                console.error('Error fetching books:', error);
                this.books = [];
            } finally {
                this.loading = false;
            }
        },
        handleSearch() {
            this.currentPage = 1;
            this.fetchBooks();
        },
        handleSortChange() {
            this.currentPage = 1;
            this.fetchBooks();
        },
        handleFilterChange() {
            this.currentPage = 1;
            this.fetchBooks();
        },
        resetFilters() {
            this.searchQuery = '';
            this.filterValues = {
                minPrice: null,
                maxPrice: null,
                inStock: false,
                brand: '',
                supplier: ''
            };
            this.sortBy = 'title';
            this.currentPage = 1;
            this.fetchBooks();
        },
        nextPage() {
            if (this.currentPage < this.totalPages) {
                this.currentPage++;
                this.fetchBooks();
                window.scrollTo({ top: 0, behavior: 'smooth' });
            }
        },
        previousPage() {
            if (this.currentPage > 1) {
                this.currentPage--;
                this.fetchBooks();
                window.scrollTo({ top: 0, behavior: 'smooth' });
            }
        },
        goToPage(page) {
            this.currentPage = page;
            this.fetchBooks();
            window.scrollTo({ top: 0, behavior: 'smooth' });
        }
    },
    mounted() {
        this.fetchBooks();
        }

};
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

    .sort-select, .filter-input {
        padding: 8px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 0.95rem;
    }

    .price-filter {
        display: flex;
        gap: 4px;
        align-items: center;
    }

    .price-input {
        flex: 1;
        padding: 6px 8px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 0.85rem;
        width: 0; /* Allow flex to control width */
    }

    .price-separator {
        color: #999;
        font-size: 0.9rem;
        padding: 0 2px;
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

    .reset-btn {
        padding: 8px 12px;
        background: #e8e8e8;
        border: 1px solid #ccc;
        border-radius: 4px;
        cursor: pointer;
        font-size: 0.95rem;
        margin-top: 8px;
    }

    .reset-btn:hover {
        background: #ddd;
    }

    /* Main area */
    .main {
        background: transparent;
        padding: 18px;
    }

    .main-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 18px;
    }

    .results-info {
        font-size: 0.95rem;
        color: #666;
    }

    .loading, .no-results {
        text-align: center;
        padding: 40px;
        color: #666;
        font-size: 1.1rem;
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
        border: 2px solid #42b983;
        background: white;
        padding: 8px 16px;
        color: #42b983;
        cursor: pointer;
        font-weight: 600;
        font-size: 0.85rem;
        transition: all 0.2s;
    }

    .buy:hover:not(:disabled) {
        background: #42b983;
        color: white;
    }

    .buy:disabled {
        border-color: #ddd;
        color: #999;
        cursor: not-allowed;
        background: #f5f5f5;
    }

    .description {
        max-height: 40px; 
        overflow: hidden;
        font-size: 0.9rem;
        color: #444;
        margin: 8px 0;
    }

    .card:hover .description {
        overflow: visible;
        max-height: 400px;
    }

    .stock {
        font-size: 0.9rem;
        color: #666;
    }

    .stock.out-of-stock {
        color: #d9534f;
        font-weight: 600;
    }

    /* Pagination */
    .pagination {
        display: flex;
        justify-content: center;
        align-items: center;
        gap: 12px;
        margin-top: 30px;
        padding: 20px;
    }

    .btn-page {
        padding: 8px 12px;
        background: #f5f5f5;
        border: 1px solid #ddd;
        border-radius: 4px;
        cursor: pointer;
        font-size: 0.95rem;
    }

    .btn-page:hover:not(:disabled) {
        background: #e8e8e8;
    }

    .btn-page:disabled {
        opacity: 0.5;
        cursor: not-allowed;
    }

    .page-numbers {
        display: flex;
        gap: 4px;
    }

    .btn-page-number {
        padding: 6px 10px;
        background: white;
        border: 1px solid #ddd;
        border-radius: 4px;
        cursor: pointer;
        font-size: 0.9rem;
        min-width: 36px;
    }

    .btn-page-number:hover {
        background: #f5f5f5;
    }

    .btn-page-number.active {
        background: var(--accent);
        color: white;
        border-color: var(--accent);
    }

    .nav {
        display: flex;
        align-items: center;
        gap: 20px;
        padding: 8px 16px;
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

    @media (max-width: 900px) {
        .content {
            grid-template-columns: 1fr;
        }

        .sidebar {
            order: 2;
        }

        .main {
            order: 1;
        }

        .card img {
            height: 160px;
        }

        .main-header {
            flex-direction: column;
            align-items: flex-start;
            gap: 10px;
        }
    }
    .card.clickable {
        cursor: pointer;
    }

</style>
