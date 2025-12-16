<template>
    <div class="product-page" v-if="book">
        <div class="image">
            <img :src="book.imageUrl || fallbackImage" />
        </div>

        <div class="info">
            <h1>{{ book.title }}</h1>

            <p class="price">{{ book.price }}&nbsp;&#8364;</p>

            <div class="stock" :class="{ out: !book.isAvailable }">
                {{ book.isAvailable ? 'In stock' : 'Out of stock' }}
            </div>

            <button class="buy">Add to cart</button>

            <h3>Description</h3>
            <p>{{ book.longDescription }}</p>

            <h3>Technical details</h3>
            <pre>{{ book.technicalDetails }}</pre>
        </div>
    </div>

</template>

<script>
    import booksService from '@/services/mock-books-service';
    //import booksService from '@/services/books-service'; tole bomo dali ko bo backend delu haha


    export default {
        name: 'BookDetailView',
        props: ['id'],
        data() {
            return {
                book: null,
                fallbackImage: 'https://via.placeholder.com/400x500?text=No+Image'
            };
        },
        async mounted() {
            try {
                this.book = await booksService.getBookById(this.id); 
            } catch (e) {
                console.error(e);
            }
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
    .buy:disabled {
        background: #ccc;
        cursor: not-allowed;
    }

    .buy {
        border-radius: 18px;
        border: 2px solid #cfcfcf;
        background: transparent;
        padding: 10px 20px;
        color: #777;
        cursor: pointer;
        margin: 20px 0 0 0;
    }

    .loading {
        padding: 40px;
        text-align: center;
    }
</style>
