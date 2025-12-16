import { reactive } from 'vue';

class CartService {
  constructor() {
    // Load cart from localStorage or initialize empty
    const savedCart = localStorage.getItem('cart');
    this.state = reactive({
      items: savedCart ? JSON.parse(savedCart) : []
    });
  }

  /**
   * Get cart state
   */
  getState() {
    return this.state;
  }

  /**
   * Add item to cart
   */
  addItem(book, quantity = 1) {
    const existingItem = this.state.items.find(item => item.id === book.id);
    
    if (existingItem) {
      existingItem.quantity += quantity;
    } else {
      this.state.items.push({
        id: book.id,
        title: book.title,
        price: book.price,
        image: book.image,
        stock: book.stock,
        quantity: quantity
      });
    }
    
    this.saveCart();
  }

  /**
   * Remove item from cart
   */
  removeItem(bookId) {
    const index = this.state.items.findIndex(item => item.id === bookId);
    if (index !== -1) {
      this.state.items.splice(index, 1);
      this.saveCart();
    }
  }

  /**
   * Update item quantity
   */
  updateQuantity(bookId, quantity) {
    const item = this.state.items.find(item => item.id === bookId);
    if (item) {
      if (quantity <= 0) {
        this.removeItem(bookId);
      } else {
        item.quantity = quantity;
        this.saveCart();
      }
    }
  }

  /**
   * Get cart item count
   */
  getItemCount() {
    return this.state.items.reduce((total, item) => total + item.quantity, 0);
  }

  /**
   * Get cart subtotal
   */
  getSubtotal() {
    return this.state.items.reduce((total, item) => total + (item.price * item.quantity), 0);
  }

  /**
   * Get shipping cost
   */
  getShipping() {
    const subtotal = this.getSubtotal();
    return subtotal > 50 ? 0 : 5; // Free shipping over 50€
  }

  /**
   * Get cart total
   */
  getTotal() {
    return this.getSubtotal() + this.getShipping();
  }

  /**
   * Clear cart
   */
  clearCart() {
    this.state.items = [];
    this.saveCart();
  }

  /**
   * Check if item is in cart
   */
  isInCart(bookId) {
    return this.state.items.some(item => item.id === bookId);
  }

  /**
   * Get item quantity in cart
   */
  getItemQuantity(bookId) {
    const item = this.state.items.find(item => item.id === bookId);
    return item ? item.quantity : 0;
  }

  /**
   * Save cart to localStorage
   */
  saveCart() {
    localStorage.setItem('cart', JSON.stringify(this.state.items));
  }
}

export default new CartService();
