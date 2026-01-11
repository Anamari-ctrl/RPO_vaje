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
                                <button @click="decreaseQuantity(item)" class="qty-btn">-</button>
                                <span class="quantity">{{ item.quantity }}</span>
                                <button @click="increaseQuantity(item)" class="qty-btn">+</button>
                            </div>
                            <button @click="removeItem(item.id)" class="btn-remove">Remove</button>
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

                    <button @click="checkout" class="btn-checkout" :disabled="isProcessing">
                        {{ isProcessing ? 'Processing...' : 'CHECKOUT' }}
                    </button>

                    <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>

                    <p class="international-note">
                        For international orders, please email <a href="mailto:info@owls.com">info@owls.com</a>.
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { computed, ref } from 'vue';
import { useRouter } from 'vue-router';
import cartService from '../services/cart-service';
import orderService from '../services/order-service';
import accountService from '../services/account-service';

export default {
  name: 'CartView',
  setup() {
    const router = useRouter();
    const cartState = cartService.getState();
    const isProcessing = ref(false);
    const errorMessage = ref('');

    const cartItems = computed(() => cartState.items);
    const subtotal = computed(() => cartService.getSubtotal());
    const shipping = computed(() => cartService.getShipping());
    const total = computed(() => cartService.getTotal());

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

    const checkout = async () => {
      errorMessage.value = '';
      
      const accountState = accountService.getState();
      if (!accountState.isUserLoggedIn || !accountState.currentUserId) {
        errorMessage.value = 'Please log in to proceed with checkout';
        setTimeout(() => {
          router.push('/login');
        }, 1500);
        return;
      }

      if (cartItems.value.length === 0) {
        errorMessage.value = 'Your cart is empty';
        return;
      }

      isProcessing.value = true;
      try {
        const orderData = {
          userId: accountState.currentUserId,
          orderDate: new Date().toISOString(),
          totalAmount: total.value,
          shippingCost: shipping.value,
          orderItems: cartItems.value.map(item => ({
            productId: item.id,
            quantity: item.quantity,
            unitPrice: item.price
          }))
        };

        const response = await orderService.createOrder(orderData);
        
        cartService.clearCart();
        
        router.push(`/profile`);
        
        const orderId = response.id || response.orderId || 'successfully created';
        alert(`Order ${orderId}! Your order has been placed.`);
      } catch (err) {
        console.error('Failed to create order:', err);
        errorMessage.value = err.message || 'Failed to create order. Please try again.';
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
      checkout,
      isProcessing,
      errorMessage
    };
  }
};
</script>

<style scoped>
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
