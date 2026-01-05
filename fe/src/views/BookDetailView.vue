<template>
        <nav class="nav">
            <div class="nav-1">
                <router-link class="item" to="/profile">👤 Profile</router-link>
                <router-link class="item" to="/">❤︎ Wishlist</router-link>
            </div>
        </nav>
        <div v-if="loading" class="loading">
            Loading book details...
        </div>

        <div class="product-page" v-else-if="book">
            <div class="image">
                <img :src="book.imageUrl || fallbackImage" />
            </div>

            <div class="info">
                <h1>{{ book.title }}</h1>

                <p class="price">{{ book.price }} €</p>

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

                <!-- Ratings Section -->
                <div class="ratings-section">
                    <h2>Rate this product</h2>
                    <div v-if="isLoggedIn" class="my-rating">
                        <p v-if="myRating">You have already rated this product. Update your review:</p>
                        <div class="rating-form">
                            <div class="star-input">
                                <span v-for="n in 5" :key="n"
                                      class="star clickable"
                                      :class="{ filled: n <= userRatingValue }"
                                      @click="userRatingValue = n">★</span>
                            </div>
                            <textarea v-model="userComment"
                                      placeholder="Share your thoughts about this product..."
                                      rows="4"></textarea>
                            <div class="form-actions">
                                <button @click="submitRating" :disabled="userRatingValue === 0">
                                    {{ myRating ? 'Update Review' : 'Submit Review' }}
                                </button>
                                <button v-if="myRating" @click="deleteRating" class="delete-btn">
                                    Delete Review
                                </button>
                            </div>
                            <p v-if="ratingMessage" class="message" :class="{ error: ratingError }">
                                {{ ratingMessage }}
                            </p>
                        </div>
                    </div>
                    <div v-else class="login-prompt">
                        <p>Please <router-link to="/login">log in</router-link> to rate this product.</p>
                    </div>
                </div>
            </div>
        </div>

        <div v-else class="loading">
            Book not found.
        </div>
    </template>

    <script>
        import { ref, onMounted, computed } from "vue";
        import { useRoute } from "vue-router";
        import booksService from "@/services/books-service";
        import cartService from "@/services/cart-service";
        import ratingService from "@/services/rating-service";
        import accountService from "@/services/account-service";

        export default {
            name: "BookDetailView",
            setup() {
                const route = useRoute();

                const book = ref(null);
                const loading = ref(true);
                const myRating = ref(null);
                const userRatingValue = ref(0);
                const userComment = ref("");
                const ratingMessage = ref("");
                const ratingError = ref(false);

                const accountState = accountService.getState();
                const isLoggedIn = computed(() => accountState.isUserLoggedIn);
                const currentUserId = computed(() => accountState.currentUserId);

                const fallbackImage =
                    "https://via.placeholder.com/400x500?text=No+Image";

                const loadBook = async () => {
                    loading.value = true;
                    try {
                        const id = route.params.id;
                        book.value = await booksService.getBookById(id);
                        if (isLoggedIn.value && book.value) {
                            await loadMyRating();
                        }
                    } catch (err) {
                        console.error("Failed to load book:", err);
                        book.value = null;
                    } finally {
                        loading.value = false;
                    }
                };

                const loadMyRating = async () => {
                    const storageKey = `rating_${book.value.id}_${currentUserId.value}`;
                    const storedRating = localStorage.getItem(storageKey);

                    if (storedRating) {
                        const rating = JSON.parse(storedRating);
                        myRating.value = rating;
                        userRatingValue.value = rating.ratingValue;
                        userComment.value = rating.comment;
                    }
                };

                const submitRating = async () => {
                    if (userRatingValue.value === 0) {
                        ratingMessage.value = "Please select a star rating";
                        ratingError.value = true;
                        return;
                    }
                    if (!currentUserId.value) {
                        ratingMessage.value = "User ID not found. Please log in again.";
                        ratingError.value = true;
                        return;
                    }
                    ratingMessage.value = "";
                    ratingError.value = false;
                    try {
                        const storageKey = `rating_${book.value.id}_${currentUserId.value}`;

                        if (myRating.value) {
                            await ratingService.updateRating({
                                ratingId: myRating.value.ratingId,
                                productId: book.value.id,
                                userId: currentUserId.value,
                                ratingValue: userRatingValue.value,
                                comment: userComment.value
                            });
                            ratingMessage.value = "Review updated successfully!";
                        } else {
                            const newRating = await ratingService.createRating({
                                productId: book.value.id,
                                userId: currentUserId.value,
                                ratingValue: userRatingValue.value,
                                comment: userComment.value
                            });
                            myRating.value = newRating;
                            ratingMessage.value = "Review submitted successfully!";
                        }

                        // Store in localStorage for persistence
                        localStorage.setItem(storageKey, JSON.stringify({
                            ratingId: myRating.value.ratingId,
                            productId: book.value.id,
                            userId: currentUserId.value,
                            ratingValue: userRatingValue.value,
                            comment: userComment.value
                        }));

                        ratingError.value = false;
                    } catch (err) {
                        console.error("Failed to submit rating:", err);
                        ratingMessage.value = err.message || "Failed to submit review";
                        ratingError.value = true;
                    }
                };

                const deleteRating = async () => {
                    if (!confirm("Are you sure you want to delete your review?")) {
                        return;
                    }
                    try {
                        await ratingService.deleteRating(myRating.value.ratingId);
                        const storageKey = `rating_${book.value.id}_${currentUserId.value}`;
                        localStorage.removeItem(storageKey);

                        userRatingValue.value = 0;
                        userComment.value = "";
                        myRating.value = null;
                        ratingMessage.value = "Review deleted successfully!";
                        ratingError.value = false;
                    } catch (err) {
                        console.error("Failed to delete rating:", err);
                        ratingMessage.value = err.message || "Failed to delete review";
                        ratingError.value = true;
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
                    addToCart,
                    myRating,
                    userRatingValue,
                    userComment,
                    ratingMessage,
                    ratingError,
                    isLoggedIn,
                    submitRating,
                    deleteRating
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

        .image {
            width: 100%;
            max-width: 400px;
        }

            .image img {
                width: 100%;
                height: 500px; /* vedno enak okvir */
                object-fit: cover;
                border-radius: 12px;
                background: #f0f0f0; /* če ni slike, bo siva površina */
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

        /* Rating styles */
        .ratings-section {
            margin-top: 40px;
            padding-top: 30px;
            border-top: 1px solid #e0e0e0;
        }

            .ratings-section h2 {
                margin-bottom: 20px;
                color: #333;
            }

        .my-rating {
            background: #f9f9f9;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 30px;
        }

        .rating-form {
            margin-top: 15px;
        }

        .star-input {
            font-size: 2rem;
            margin-bottom: 15px;
        }

        .star {
            color: #ddd;
            transition: color 0.2s;
        }

            .star.filled {
                color: #ffd700;
            }

            .star.clickable {
                cursor: pointer;
            }

                .star.clickable:hover {
                    color: #ffed4e;
                }

        textarea {
            width: 100%;
            padding: 12px;
            border: 1px solid #ddd;
            border-radius: 6px;
            font-family: inherit;
            font-size: 0.95rem;
            resize: vertical;
            margin-bottom: 15px;
        }

            textarea:focus {
                outline: none;
                border-color: #4CAF50;
            }

        .form-actions {
            display: flex;
            gap: 10px;
        }

            .form-actions button {
                padding: 10px 20px;
                border: none;
                border-radius: 6px;
                cursor: pointer;
                font-weight: 600;
                transition: background 0.2s;
            }

                .form-actions button:disabled {
                    background: #ccc;
                    color: #666;
                    cursor: not-allowed;
                }

                .form-actions button:not(:disabled) {
                    background: #4CAF50;
                    color: white;
                }

                    .form-actions button:not(:disabled):hover {
                        background: #45a049;
                    }

            .form-actions .delete-btn {
                background: #f44336;
            }

                .form-actions .delete-btn:hover {
                    background: #da190b;
                }

        .message {
            margin-top: 10px;
            padding: 10px;
            border-radius: 4px;
            background: #d4edda;
            color: #155724;
        }

            .message.error {
                background: #f8d7da;
                color: #721c24;
            }

        .login-prompt {
            padding: 20px;
            background: #fff3cd;
            border-radius: 6px;
            color: #856404;
        }

            .login-prompt a {
                color: #0066cc;
                text-decoration: underline;
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

    </style>
