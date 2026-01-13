import accountService from "./account-service";
import booksService from "./books-service";

const API_BASE_URL = "http://localhost:7020/api/v1";

class OrderService {
    // helper: safe number
    _num(v) {
        const n = Number(v);
        return Number.isFinite(n) ? n : 0;
    }

    // helper: map raw order from backend
    _mapOrder(o) {
        const rawItems = o.orderItems ?? o.OrderItems ?? o.items ?? o.Items ?? [];

        return {
            id: o.orderId ?? o.OrderId,
            number: o.orderNumber ?? o.OrderNumber ?? (o.orderId ?? o.OrderId),
            createdAt: o.orderDate ?? o.OrderDate ?? null,
            status: o.orderStatus ?? o.OrderStatus ?? "In progress",

            total: this._num(o.totalAmount ?? o.TotalAmount),

            // backend ti v response-u trenutno sploh ne pošilja deliveryCost,
            // zato tukaj pustimo 0 in bomo kasneje izračunali, če je možno.
            shipping: this._num(o.deliveryCost ?? o.DeliveryCost ?? 0),

            items: Array.isArray(rawItems)
                ? rawItems.map((i) => ({
                    orderItemId: i.orderItemId ?? i.OrderItemId,
                    productId: i.productId ?? i.ProductId,
                    quantity: this._num(i.quantity ?? i.Quantity),
                    priceAtPurchase: this._num(i.priceAtPurchase ?? i.PriceAtPurchase ?? i.price ?? i.Price),

                    productName: i.productName ?? i.ProductName ?? null,
                    shortDescription: i.shortDescription ?? i.ShortDescription ?? null,
                }))
                : [],
        };
    }

    // ENRICH: fill missing productName/shortDescription/priceAtPurchase using booksService
    async _enrichOrdersWithProducts(orders) {
        // collect productIds that need enrichment
        const ids = new Set();
        for (const o of orders) {
            for (const it of o.items) {
                if (it.productId) ids.add(it.productId);
            }
        }

        const productMap = new Map();

        await Promise.all(
            Array.from(ids).map(async (id) => {
                try {
                    const b = await booksService.getBookById(id);
                    if (b) productMap.set(id, b);
                } catch {
                    // ignore if fails
                }
            })
        );

        for (const o of orders) {
            for (const it of o.items) {
                const b = productMap.get(it.productId);
                if (!b) continue;

                // fill missing
                if (!it.productName) it.productName = b.title ?? b.name ?? "Unknown Product";
                if (!it.shortDescription) it.shortDescription = b.shortDescription ?? b.description ?? "";

                // if backend returned 0 priceAtPurchase, fallback to current product price for display
                if (!it.priceAtPurchase || it.priceAtPurchase === 0) {
                    it.priceAtPurchase = this._num(b.price);
                }
            }

            // if shipping not present in response, infer from total - itemsSum (non-negative)
            if (!o.shipping || o.shipping === 0) {
                const itemsSum = o.items.reduce(
                    (acc, it) => acc + this._num(it.priceAtPurchase) * this._num(it.quantity),
                    0
                );
                const inferred = Math.max(0, this._num(o.total) - itemsSum);
                // samo če je smiselno (da ni 0 zaradi napačnih cen)
                if (inferred > 0) o.shipping = inferred;
            }
        }

        return orders;
    }

    // ORDER HISTORY for current user
    async getMyOrders() {
        const headers = accountService.getHeaderData();

        const response = await fetch(`${API_BASE_URL}/users/orderHistory`, {
            method: "GET",
            headers,
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Failed to fetch orders");
        }

        const data = await response.json();
        const rawList = Array.isArray(data) ? data : [];

        // map correctly (uses orderItems!)
        const orders = rawList.map((o) => this._mapOrder(o));

        // enrich items for display (because API currently returns null/0)
        return await this._enrichOrdersWithProducts(orders);
    }

    // Create new order
    async createOrder(orderData) {
        let headers = {};
        try {
            headers = accountService.getHeaderData() || {};
        } catch {
            headers = {};
        }
        headers["Content-Type"] = "application/json";

        const response = await fetch(`${API_BASE_URL}/orders/create`, {
            method: "POST",
            headers,
            body: JSON.stringify(orderData),
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Failed to create order");
        }

        return await response.json();
    }
}

export default new OrderService();
