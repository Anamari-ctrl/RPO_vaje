<template>
    <nav class="nav">
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
    </nav>

    <div class="cart-container">
        <div class="cart-content">
            <h1 class="cart-title">Cart</h1>

            <div v-if="cartItems.length === 0" class="empty-cart">
                <p>Your cart is empty</p>
                <router-link to="/" class="btn-continue">Continue Shopping</router-link>
            </div>

            <div v-else class="cart-layout">
                <!-- Cart Items -->
                <div class="cart-items">
                    <div v-for="item in cartItems" :key="item.id" class="cart-item">
                        <img :src="item.image || 'https://via.placeholder.com/120x160?text=Book'" :alt="item.title" class="item-image" />

                        <div class="item-details">
                            <h3 class="item-title">{{ item.title }}</h3>
                            <p class="item-price">{{ item.price }} €</p>
                        </div>

                        <div class="item-actions">
                            <div class="quantity-controls">
                                <button type="button" @click="decreaseQuantity(item)" class="qty-btn">-</button>
                                <span class="quantity">{{ item.quantity }}</span>
                                <button type="button" @click="increaseQuantity(item)" class="qty-btn">+</button>
                            </div>
                            <button type="button" @click="removeItem(item.id)" class="btn-remove">Remove</button>
                        </div>
                    </div>
                </div>

                <!-- Order Summary -->
                <div class="order-summary">
                    <h2>Order Summary</h2>

                    <div class="summary-row">
                        <span>Subtotal</span>
                        <span>{{ subtotal.toFixed(2) }}€</span>
                    </div>

                    <div class="summary-row">
                        <span>Estimated Shipping</span>
                        <span>{{ shipping === 0 ? 'Free shipping' : shipping.toFixed(2) + '€' }}</span>
                    </div>

                    <hr class="summary-divider" />

                    <div class="summary-row total">
                        <span><strong>Order Total:</strong></span>
                        <span><strong>{{ total.toFixed(2) }}€</strong></span>
                    </div>

                    <!-- IMPORTANT: open modal only -->
                    <button type="button"
                            class="btn-checkout"
                            :disabled="isProcessing"
                            @click.stop.prevent="openCheckout">
                        {{ isProcessing ? 'Processing...' : 'CHECKOUT' }}
                    </button>

                    <!-- MODAL -->
                    <div v-if="showCheckoutModal" class="modal-backdrop" @click.self="closeCheckout">
                        <div class="modal" role="dialog" aria-modal="true" aria-label="Checkout modal">
                            <div class="modal-header">
                                <h3>Checkout</h3>
                                <button type="button" class="modal-close" @click="closeCheckout" aria-label="Close">✕</button>
                            </div>

                            <div class="modal-body">
                                <h4>Payment method</h4>

                                <div class="pay-grid">
                                    <label class="pay-option">
                                        <input type="radio" value="CreditCard" v-model="paymentType" />
                                        Credit card
                                    </label>

                                    <label class="pay-option">
                                        <input type="radio" value="CashOnDelivery" v-model="paymentType" />
                                        Cash on delivery
                                    </label>

                                    <label class="pay-option">
                                        <input type="radio" value="Paypal" v-model="paymentType" />
                                        PayPal
                                    </label>
                                </div>

                                <!-- Guest delivery details -->
                                <div v-if="!isLoggedIn" class="guest-shipping">
                                    <h4>Delivery details</h4>

                                    <input v-model.trim="shippingForm.fullName" class="input" type="text" placeholder="Full name" />
                                    <input v-model.trim="shippingForm.email" class="input" type="email" placeholder="Email" />

                                    <input v-model.trim="shippingForm.address" class="input" type="text" placeholder="Address" />

                                    <div class="row">
                                        <input v-model.trim="shippingForm.city" class="input" type="text" placeholder="City" />
                                        <input v-model.trim="shippingForm.postalCode" class="input" type="text" placeholder="Postal code" />
                                    </div>

                                    <input v-model.trim="shippingForm.country" class="input" type="text" placeholder="Country" />
                                </div>

                                <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
                            </div>

                            <div class="modal-footer">
                                <button type="button" class="btn-secondary" @click.stop.prevent="closeCheckout">
                                    Cancel
                                </button>

                                <!-- IMPORTANT: order is sent only here -->
                                <button type="button"
                                        class="btn-primary"
                                        @click.stop.prevent="confirmCheckout"
                                        :disabled="isProcessing">
                                    Confirm order
                                </button>
                            </div>
                        </div>
                    </div>

                    <p class="international-note">
                        For international orders, please email <a href="mailto:info@owls.com">info@owls.com</a>.
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { computed, ref } from "vue";
    import { useRouter } from "vue-router";
    import cartService from "../services/cart-service";
    import orderService from "../services/order-service";
    import booksService from "../services/books-service";

    export default {
        name: "CartView",
        setup() {
            const router = useRouter();

            const cartState = cartService.getState();

            const isProcessing = ref(false);
            const errorMessage = ref("");

            const cartItems = computed(() => cartState.items);
            const subtotal = computed(() => cartService.getSubtotal());
            const shipping = computed(() => cartService.getShipping());
            const total = computed(() => cartService.getTotal());

            // Logged in = token + userId in localStorage (NE redirectaj nikamor)
            const isLoggedIn = computed(() => {
                const token = localStorage.getItem("token");
                const userId = localStorage.getItem("userId");
                return !!token && !!userId;
            });

            const showCheckoutModal = ref(false);
            const paymentType = ref("CreditCard");

            const shippingForm = ref({
                fullName: "",
                email: "",
                address: "",
                city: "",
                postalCode: "",
                country: "Slovenia",
            });

            const increaseQuantity = (item) => {
                if (item.quantity < item.stock) {
                    cartService.updateQuantity(item.id, item.quantity + 1);
                }
            };

            const decreaseQuantity = (item) => {
                cartService.updateQuantity(item.id, item.quantity - 1);
            };

            const removeItem = (itemId) => {
                cartService.removeItem(itemId);
            };

            // ONLY opens modal
            const openCheckout = () => {
                errorMessage.value = "";

                if (cartItems.value.length === 0) {
                    errorMessage.value = "Your cart is empty";
                    return;
                }

                showCheckoutModal.value = true;
            };

            const closeCheckout = () => {
                showCheckoutModal.value = false;
                errorMessage.value = "";
            };

            // ONLY sends order here
            const confirmCheckout = async () => {
                if (isProcessing.value) return;

                errorMessage.value = "";

                if (cartItems.value.length === 0) {
                    errorMessage.value = "Your cart is empty";
                    return;
                }

                const userId = isLoggedIn.value ? localStorage.getItem("userId") : null;

                // Guest must fill delivery form
                if (!userId) {
                    const f = shippingForm.value;

                    const missing =
                        !f.fullName ||
                        !f.email ||
                        !f.address ||
                        !f.city ||
                        !f.postalCode ||
                        !f.country;

                    if (missing) {
                        errorMessage.value = "Please fill in delivery details";
                        return;
                    }
                }

                isProcessing.value = true;

                try {
                    // IMPORTANT: use the backend field names EXACTLY
                    const orderData = {
                        UserId: userId, // null for guest
                        OrderDate: new Date().toISOString(),
                        TotalAmount: total.value,
                        DeliveryCost: shipping.value,
                        PaymentType: paymentType.value,

                        Address: userId ? null : shippingForm.value.address,
                        City: userId ? null : shippingForm.value.city,
                        Country: userId ? null : shippingForm.value.country,
                        PostalCode: userId ? null : shippingForm.value.postalCode,

                        OrderItems: cartItems.value.map((item) => ({
                            ProductId: item.id,
                            Quantity: item.quantity,
                            PriceAtPurchase: item.price,
                        })),
                    };

                    const response = await orderService.createOrder(orderData);

                    // Zmanjšaj stock za vsak izdelek v naročilu
                    for (const item of cartItems.value) {
                        try {
                            await booksService.decreaseStock(item.id, item.quantity);
                        } catch (stockError) {
                            console.warn(`Failed to decrease stock for ${item.title}:`, stockError);
                            // Nadaljuj kljub napaki pri zmanjšanju stock-a
                        }
                    }

                    cartService.clearCart();
                    closeCheckout();

                    // če imaš profile page, naj gre tja
                    router.push("/profile");

                    const orderId = response?.orderId || response?.id || "created";
                    alert(`Order ${orderId}! Your order has been placed.`);
                } catch (err) {
                    console.error(err);
                    errorMessage.value = err?.message || "Failed to create order.";
                } finally {
                    isProcessing.value = false;
                }
            };

            return {
                cartItems,
                subtotal,
                shipping,
                total,

                increaseQuantity,
                decreaseQuantity,
                removeItem,

                isProcessing,
                errorMessage,

                isLoggedIn,
                showCheckoutModal,
                paymentType,
                shippingForm,

                openCheckout,
                closeCheckout,
                confirmCheckout,
            };
        },
    };
</script>

<style scoped>
    /* modal */
    .modal-backdrop {
        position: fixed;
        inset: 0;
        background: rgba(0, 0, 0, 0.45);
        display: flex;
        align-items: flex-start;
        justify-content: center;
        padding: 20px 12px;
        z-index: 9999;
    }

    .modal {
        width: 100%;
        max-width: 520px;
        background: #fff;
        border-radius: 14px;
        box-shadow: 0 20px 60px rgba(0, 0, 0, 0.25);
        overflow: hidden;
        margin-top: 60px;
    }

    .modal-header,
    .modal-footer {
        padding: 14px 16px;
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 12px;
        border-bottom: 1px solid #eee;
    }

    .modal-footer {
        border-bottom: none;
        border-top: 1px solid #eee;
    }

    .modal-body {
        padding: 16px;
    }

    .modal-close {
        border: none;
        background: transparent;
        cursor: pointer;
        font-size: 18px;
    }

    .pay-grid {
        display: grid;
        grid-template-columns: 1fr;
        gap: 10px;
        margin: 10px 0 18px 0;
    }

    .pay-option {
        display: flex;
        align-items: center;
        gap: 10px;
        padding: 10px 12px;
        border: 1px solid #e6e6e6;
        border-radius: 12px;
        cursor: pointer;
    }

    .guest-shipping {
        margin-top: 14px;
    }

    .input {
        width: 100%;
        padding: 10px 12px;
        border: 1px solid #ddd;
        border-radius: 10px;
        margin-bottom: 10px;
    }

    .row {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 10px;
    }

    .btn-secondary {
        padding: 10px 14px;
        border-radius: 10px;
        border: 1px solid #ddd;
        background: #fff;
        cursor: pointer;
    }

    .btn-primary {
        padding: 10px 14px;
        border-radius: 10px;
        border: none;
        background: #4169e1;
        color: #fff;
        cursor: pointer;
    }

    @media (max-width: 520px) {
        .modal {
            margin-top: 20px;
        }

        .row {
            grid-template-columns: 1fr;
        }
    }

    /* page */
    .cart-container {
        min-height: 80vh;
        padding: 20px;
        background: #f9f9f9;
    }

    .cart-content {
        max-width: 1200px;
        margin: 0 auto;
    }

    .cart-title {
        font-size: 2rem;
        margin-bottom: 30px;
        color: #333;
    }

    .empty-cart {
        text-align: center;
        padding: 60px 20px;
        background: white;
        border-radius: 8px;
    }

        .empty-cart p {
            font-size: 1.2rem;
            color: #666;
            margin-bottom: 20px;
        }

    .btn-continue {
        display: inline-block;
        padding: 12px 24px;
        background: #42b983;
        color: white;
        text-decoration: none;
        border-radius: 6px;
        font-weight: 600;
    }

        .btn-continue:hover {
            background: #359268;
        }

    .cart-layout {
        display: grid;
        grid-template-columns: 1fr 400px;
        gap: 30px;
        align-items: start;
    }

    .cart-items {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }

    .cart-item {
        display: flex;
        gap: 20px;
        background: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
    }

    .item-image {
        width: 120px;
        height: 160px;
        object-fit: cover;
        border-radius: 6px;
    }

    .item-details {
        flex: 1;
    }

    .item-title {
        font-size: 1.1rem;
        margin: 0 0 10px 0;
        color: #333;
    }

    .item-price {
        font-size: 1.2rem;
        color: #ff6d2f;
        font-weight: 700;
        margin: 0;
    }

    .item-actions {
        display: flex;
        flex-direction: column;
        gap: 10px;
        align-items: flex-end;
    }

    .quantity-controls {
        display: flex;
        align-items: center;
        gap: 12px;
        border: 1px solid #ddd;
        border-radius: 6px;
        padding: 4px 8px;
    }

    .qty-btn {
        background: none;
        border: none;
        font-size: 1.2rem;
        cursor: pointer;
        padding: 4px 8px;
        color: #666;
    }

        .qty-btn:hover {
            color: #42b983;
        }

    .quantity {
        font-size: 1rem;
        font-weight: 600;
        min-width: 30px;
        text-align: center;
    }

    .btn-remove {
        background: none;
        border: none;
        color: #c33;
        font-size: 0.9rem;
        cursor: pointer;
        text-decoration: underline;
    }

        .btn-remove:hover {
            color: #a00;
        }

    .order-summary {
        background: white;
        padding: 30px;
        border-radius: 8px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        position: sticky;
        top: 20px;
    }

        .order-summary h2 {
            font-size: 1.4rem;
            margin-top: 0;
            margin-bottom: 20px;
            color: #333;
        }

    .summary-row {
        display: flex;
        justify-content: space-between;
        margin-bottom: 15px;
        font-size: 1rem;
        color: #555;
    }

        .summary-row.total {
            font-size: 1.2rem;
            color: #333;
            margin-top: 15px;
        }

    .summary-divider {
        border: none;
        border-top: 1px solid #ddd;
        margin: 20px 0;
    }

    .btn-checkout {
        width: 100%;
        padding: 14px;
        background: #4169e1;
        color: white;
        border: none;
        border-radius: 6px;
        font-size: 1rem;
        font-weight: 700;
        cursor: pointer;
        margin-top: 20px;
        letter-spacing: 0.5px;
        transition: background 0.2s;
    }

        .btn-checkout:hover:not(:disabled) {
            background: #3557c7;
        }

        .btn-checkout:disabled {
            background: #999;
            cursor: not-allowed;
            opacity: 0.7;
        }

    .error-message {
        margin-top: 15px;
        padding: 12px;
        background: #f8d7da;
        color: #721c24;
        border-radius: 4px;
        text-align: center;
        font-size: 0.9rem;
    }

    .international-note {
        margin-top: 20px;
        font-size: 0.85rem;
        color: #666;
        text-align: center;
        line-height: 1.5;
    }

        .international-note a {
            color: #4169e1;
            text-decoration: none;
        }

            .international-note a:hover {
                text-decoration: underline;
            }

    @media (max-width: 900px) {
        .cart-layout {
            grid-template-columns: 1fr;
        }

        .order-summary {
            position: static;
        }

        .cart-item {
            flex-direction: column;
        }

        .item-actions {
            flex-direction: row;
            justify-content: space-between;
            width: 100%;
        }
    }

    /* top nav */
    .item {
        text-decoration: none;
        color: var(--text);
        font-weight: 500;
        padding: 6px 10px;
        border-radius: 6px;
        transition: background 0.2s;
    }

        .item:hover {
            background: rgba(0, 0, 0, 0.05);
        }

    .nav {
        display: flex;
        align-items: center;
        padding: 8px 16px;
        background: var(--panel);
        border-bottom: 1px solid var(--muted);
        width: 100%;
    }

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

    @media (prefers-reduced-motion: reduce) {
        .tooltip {
            transition: none;
        }
    }
</style>
