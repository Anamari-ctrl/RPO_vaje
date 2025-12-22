<template>
    <div v-if="loading" class="loading">
        Loading book details...
    </div>

    <div class="product-page" v-else-if="book">
        <div class="image">
            <img :src="book.imageUrl || fallbackImage" />
        </div>

        <div class="info">
            <h1>{{ book.title }}</h1>

            <p class="price">{{ book.price.toFixed(2) }} €</p>

            <div class="stock" :class="{ out: !book.isAvailable }">
                {{ book.isAvailable ? 'In stock' : 'Out of stock' }}
            </div>

            <button class="buy"
                    @click.stop="addToCart(book)"
                    :disabled="book.stock === 0">
                {{ book.stock === 0 ? 'OUT OF STOCK' : 'ADD TO CART' }}
            </button>

            <h3>Description</h3>
            <p>{{ book.longDescription || 'No description available.' }}</p>

            <h3>Technical details</h3>
            <pre>{{ book.technicalDetails || '—' }}</pre>
        </div>
    </div>

    <div v-else class="loading">
        Book not found.
    </div>
</template>

<script>
    import { ref, onMounted } from "vue";
    import { useRoute } from "vue-router";
    import booksService from "@/services/books-service";
    import cartService from "@/services/cart-service";

    export default {
        name: "BookDetailView",
        setup() {
            const route = useRoute();

            const book = ref(null);
            const loading = ref(true);

            const fallbackImage =
                "https://via.placeholder.com/400x500?text=No+Image";

            const loadBook = async () => {
                loading.value = true;
                try {
                    const id = route.params.id;
                    book.value = await booksService.getBookById(id);
                } catch (err) {
                    console.error("Failed to load book:", err);
                    book.value = null;
                } finally {
                    loading.value = false;
                }
            };

            const addToCart = (book) => {
                if (book.stock > 0) {
                    cartService.addItem(book, 1);
                    // Optional: Show a success message or toast notification
                    alert(`Added "${book.title}" to cart!`);
                }
            };
            onMounted(loadBook);

            return {
                book,
                loading,
                fallbackImage,
                addToCart
            };
        }
    };
</script>

<style scoped>
    .product-page {
        display: grid;
        grid-template-columns: 400px 1fr;
        gap: 40px;
        padding: 40px;
    }

    .image img {
        width: 100%;
        border-radius: 12px;
    }

    .price {
        font-size: 1.6rem;
        font-weight: bold;
        margin: 10px 0;
    }

    .stock {
        font-weight: 600;
        color: #2e7d32;
    }

        .stock.out {
            color: #c62828;
        }

    .buy {
        border-radius: 18px;
        border: 2px solid #cfcfcf;
        background: transparent;
        padding: 10px 20px;
        color: #555;
        cursor: pointer;
        margin-top: 20px;
    }

        .buy:disabled {
            background: #eee;
            color: #999;
            cursor: not-allowed;
        }

    .loading {
        padding: 40px;
        text-align: center;
        font-size: 1.2rem;
    }
</style>
