import accountService from "./account-service";

const API_BASE_URL = "http://localhost:7020/api/v1";

class OrderService {

    async getMyOrders() {
        const headers = accountService.getHeaderData();

        const response = await fetch(`${API_BASE_URL}/orders`, {
            method: "GET",
            headers: headers
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Failed to fetch orders");
        }

        return await response.json();
    }

    async getOrderById(id) {
        const headers = accountService.getHeaderData();

        const response = await fetch(`${API_BASE_URL}/orders/${id}`, {
            method: "GET",
            headers: headers
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Failed to fetch order details");
        }

        return await response.json();
    }
}

export default new OrderService();
