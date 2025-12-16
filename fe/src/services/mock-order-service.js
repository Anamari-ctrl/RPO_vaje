/**
 * Mock Order Service - Returns dummy data for testing
 * Replace with real API calls when backend is ready
 */

const MOCK_ORDERS = [
    {
        id: 1,
        userId: 1,
        createdAt: '2025-12-15T14:23:00Z',
        status: 'Delivered',
        items: [
            { productId: 1, productName: 'Essential Computer Science', price: 40, quantity: 1 },
            { productId: 5, productName: 'Python Mastery', price: 29, quantity: 2 },
        ],
        total: 98
    },
    {
        id: 2,
        userId: 1,
        createdAt: '2025-12-12T09:45:00Z',
        status: 'Shipped',
        items: [
            { productId: 3, productName: 'Algorithms and Data Structures', price: 57, quantity: 1 },
        ],
        total: 62
    },
    {
        id: 3,
        userId: 1,
        createdAt: '2025-12-10T18:10:00Z',
        status: 'Pending',
        items: [
            { productId: 7, productName: 'Web Development Guide', price: 38, quantity: 1 },
            { productId: 9, productName: 'React for Beginners', price: 45, quantity: 1 },
        ],
        total: 83
    }
];

export class MockOrderService {
    /**
     * Fetch all orders for the current user
     */
    async getMyOrders() {
        await new Promise(resolve => setTimeout(resolve, 200)); // simulate network delay
        // filter by user if needed, for now return all
        return MOCK_ORDERS;
    }

    /**
     * Fetch a single order by ID
     * @param {number|string} id
     */
    async getOrderById(id) {
        await new Promise(resolve => setTimeout(resolve, 100));
        const order = MOCK_ORDERS.find(o => o.id === Number(id));
        if (!order) throw new Error('Order not found');
        return order;
    }
}

export default new MockOrderService();
