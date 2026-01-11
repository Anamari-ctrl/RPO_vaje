import accountService from "./account-service";

const API_BASE_URL = "http://localhost:7020/api/v1";

class OrderService {

    // Pridobi zgodovino naročil trenutnega uporabnika
    async getMyOrders() {
        const headers = accountService.getHeaderData();

        const response = await fetch(`${API_BASE_URL}/users/orderHistory`, {
            method: "GET",
            headers
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Failed to fetch orders");
        }

        const data = await response.json();

        // Transformiramo polja iz backend formata v frontend
        return data.map(order => ({
            id: order.orderId,               // za Vue v-for key
            number: order.orderNumber || order.orderId, // order number za prikaz
            createdAt: order.orderDate || new Date().toISOString(),
            status: order.orderStatus || "Unknown",
            total: order.totalAmount != null ? order.totalAmount : 0,
            items: Array.isArray(order.items)
                ? order.items.map(item => ({
                    productId: item.productId || "",
                    productName: item.productName || "Unknown Product",
                    quantity: item.quantity || 0,
                    price: item.price || 0
                }))
                : []
        }));
    }

    // Ustvari novo naročilo
    async createOrder(orderData) {
        let headers = {};
        try {
            headers = accountService.getHeaderData() || {};
        } catch {
            headers = {};
        }
        headers['Content-Type'] = 'application/json';



        const response = await fetch(`${API_BASE_URL}/orders/create`, {
            method: "POST",
            headers,
            body: JSON.stringify(orderData)
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Failed to create order");
        }

        return await response.json();
    }

    // Pridobi naročilo po ID-ju
    async getOrderById(id) {
        const headers = accountService.getHeaderData();

        const response = await fetch(`${API_BASE_URL}/orders/${id}`, {
            method: "GET",
            headers
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Failed to fetch order details");
        }

        const order = await response.json();

        return {
            id: order.orderId,
            number: order.orderNumber,
            createdAt: order.orderDate || new Date().toISOString(),
            status: order.orderStatus || "Unknown",
            total: order.totalAmount != null ? order.totalAmount : 0,
            items: Array.isArray(order.items)
                ? order.items.map(item => ({
                    productId: item.productId || item.id || "",
                    productName: item.productName || item.name || "Unknown Product",
                    quantity: item.quantity != null ? item.quantity : 0,
                    price: item.price != null ? item.price : 0
                }))
                : []
        };
    }
}

export default new OrderService();
