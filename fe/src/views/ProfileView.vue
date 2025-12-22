<template>
    <h2>My Profile</h2>

    <div class="profile-container">

        <div class="button-table">
            <button class="table-btn" @click="activeScreen = 'dashboard'">Dashboard</button>
            <button class="table-btn" @click="activeScreen = 'settings'">Account Settings</button>
            <button class="table-btn" @click="activeScreen = 'orders'">Orders</button>
            <button class="table-btn" @click="logout">Logout</button>
        </div>

        <div class="content-box">

            <!-- dashboard -->
            <div v-if="activeScreen === 'dashboard'">
                <h3>Dashboard</h3>
                <p>
                    Hello {{ user.firstName || "User" }}
                    (not you? <span class="link" @click="logout">Logout</span>)
                </p>

                <p>
                    In your account dashboard you can
                    <span class="link" @click="activeScreen = 'orders'">view your recent orders</span>
                    and
                    <span class="link" @click="activeScreen = 'settings'">edit your account information</span>.
                </p>
            </div>

            <!-- Account Settings -->
            <div v-if="activeScreen === 'settings'">
                <h3>Account Settings</h3>
                <form @submit.prevent="saveProfile">

                    <div class="form-row">
                        <div class="form-group">
                            <label>First Name</label>
                            <input type="text" v-model="user.firstName" required />
                        </div>
                        <div class="form-group">
                            <label>Last Name</label>
                            <input type="text" v-model="user.lastName" required />
                        </div>
                    </div>

                    <div class="form-group">
                        <label>Email</label>
                        <input type="email" v-model="user.email" required />
                    </div>

                    <div class="form-group">
                        <label>Address</label>
                        <input type="text" v-model="user.address" />
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label>City</label>
                            <input type="text" v-model="user.city" />
                        </div>
                        <div class="form-group">
                            <label>Postal Code</label>
                            <input type="text" v-model="user.postalCode" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label>Country</label>
                        <input type="text" v-model="user.country" />
                    </div>

                    <div class="form-group">
                        <label>Phone Number</label>
                        <input type="text" v-model="user.phoneNumber" />
                    </div>

                    <h3>Change Password</h3>

                    <div class="form-group">
                        <label>Current Password</label>
                        <input type="password" v-model="user.password" />
                    </div>

                    <div class="form-group">
                        <label>New Password</label>
                        <input type="password" v-model="user.newPassword" />
                    </div>

                    <div class="form-group">
                        <label>Confirm Password</label>
                        <input type="password" v-model="user.confirmPassword" />
                    </div>

                    <button type="submit" class="btn-primary">Save Changes</button>
                </form>
            </div>

            <!-- Orders -->
            <div v-if="activeScreen === 'orders'">
                <h3>Order History</h3>
                <p v-if="orders.length === 0">You have no orders yet.</p>

                <div v-if="orders.length > 0" class="orders-list">
                    <div v-for="o in orders" :key="o.id" class="order-card">
                        <div class="order-summary" @click="toggleOrder(o)">
                            <strong>Order #{{ o.id }}</strong>
                            <span>{{ new Date(o.createdAt).toLocaleDateString() }}</span>
                            <span>{{ o.status }}</span>
                            <span>{{ o.total }} {{ '\u20AC' }}</span>
                        </div>

                        <div v-if="selectedOrder && selectedOrder.id === o.id" class="order-details">
                            <div v-for="item in selectedOrder.items" :key="item.productId" class="order-item">
                                {{ item.productName }}  | <strong>    Quantity: </strong> {{ item.quantity }}  |  <strong>   Price: </strong> {{ item.price }} {{ '\u20AC' }}
                            </div>
                            <div class="order-totals">
                                <p>
                                    Subtotal: {{ o.total }} {{ '\u20AC' }}
                                </p>
                                <p>
                                    Tax (22%): {{
 orderTax(selectedOrder)
                                    }} {{ '\u20AC' }}
                                </p>
                                <p><strong>Total: {{ orderTotal(selectedOrder) }} {{ '\u20AC' }}</strong></p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</template>

<script>
    import { ref, watch, onMounted } from "vue";
    import { useRouter } from "vue-router";
    import accountService from "../services/account-service";
    import orderService from "../services/order-service";

    export default {
        name: "ProfileView",
        setup() {
            const router = useRouter();
            const activeScreen = ref("dashboard");

            const user = ref({});
            const orders = ref([]);
            const selectedOrder = ref(null);

            // Logout
            const logout = () => {
                accountService.logout();
                router.push("/login");
            };

            // Fetch user profile
            const fetchProfile = async () => {
                try {
                    const headers = accountService.getHeaderData();
                    const response = await fetch("http://localhost:7020/api/v1/users/me", { headers });
                    if (!response.ok) throw new Error("Failed to fetch profile");
                    const data = await response.json();
                    Object.assign(user.value, data);
                } catch (err) {
                    console.error(err);
                }
            };

            // Fetch user orders
            const fetchOrders = async () => {
                try {
                    orders.value = await orderService.getMyOrders();
                } catch (err) {
                    console.error(err);
                }
            };

            const toggleOrder = (order) => {
                selectedOrder.value = selectedOrder.value && selectedOrder.value.id === order.id ? null : order;
            };

            const TAX_RATE = 0.22;
            const orderTax = (order) => ((order.total * TAX_RATE) / (1 + TAX_RATE)).toFixed(2);
            const orderSubtotal = (order) => (order.total - orderTax(order)).toFixed(2);
            const orderTotal = (order) => order.total.toFixed(2);

            // Save profile (posodobi vse podatke)
            const saveProfile = async () => {
                try {
                    const payload = {
                        UserId: user.value.id,
                        FirstName: user.value.firstName || "",
                        LastName: user.value.lastName || "",
                        Email: user.value.email || "",
                        Address: user.value.address || "",
                        City: user.value.city || "",
                        PostalCode: user.value.postalCode || "",
                        Country: user.value.country || "",
                        PhoneNumber: user.value.phoneNumber || ""
                    };


                    const headers = accountService.getHeaderData();
                    const response = await fetch("http://localhost:7020/api/v1/users/me", {
                        method: "PUT",
                        headers: {
                            ...headers,
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(payload)
                    });

                    if (!response.ok) throw new Error("Failed to save profile");

                    const updatedUser = await response.json();
                    Object.assign(user.value, updatedUser); // osveži podatke na frontendu

                    alert("Profile updated successfully!");
                } catch (err) {
                    console.error(err);
                    alert("Error saving profile. Please try again.");
                }
            };

            watch(activeScreen, (screen) => {
                if (screen === "orders") fetchOrders();
            });

            onMounted(fetchProfile);

            return {
                user,
                logout,
                activeScreen,
                orders,
                selectedOrder,
                toggleOrder,
                orderSubtotal,
                orderTax,
                orderTotal,
                saveProfile
            };
        }
    };
</script>

<style scoped>
    .profile-container {
        display: flex;
        gap: 30px;
        align-items: flex-start;
    }

    .button-table {
        display: grid;
        grid-template-columns: repeat(1, 1fr);
        width: 250px;
        border: 1px solid #ddd;
        flex-shrink: 0;
    }

    .table-btn {
        padding: 16px;
        background: #fafafa;
        border: 1px solid #ddd;
        font-size: 15px;
        cursor: pointer;
        transition: background 0.2s;
        text-align: left;
    }

        .table-btn:hover {
            background: #f0f0f0;
        }

    .content-box {
        flex: 1;
        padding: 20px;
        background: white;
        min-height: 200px;
        border-radius: 10px;
        box-shadow: 0 2px 10px rgba(0,0,0,0.08);
    }

    /* Form styling */
    .form-group {
        margin-bottom: 15px;
        margin-right: 15px;
        flex: 1;
    }

    .form-row {
        display: flex;
        gap: 30px;
        margin-bottom: 15px;
    }

    input {
        width: 100%;
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 6px;
    }

    .btn-primary {
        padding: 12px;
        background: #0077ff;
        color: white;
        border: none;
        border-radius: 8px;
        cursor: pointer;
        font-weight: bold;
    }

        .btn-primary:hover {
            background: #005fcc;
        }

    .link {
        color: #0077ff;
        cursor: pointer;
    }

        .link:hover {
            text-decoration: underline;
        }


    .orders-list {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }

    .order-card {
        border: 1px solid #ddd;
        border-radius: 8px;
        cursor: pointer;
        background: #f9f9f9;
        transition: background 0.2s;
    }

        .order-card:hover {
            background: #f0f0f0;
        }

    .order-summary {
        display: flex;
        justify-content: space-between;
        padding: 12px;
        font-weight: bold;
    }

    .order-details {
        padding: 12px;
        background: #fff;
        border-top: 1px solid #ccc;
    }

    .order-item {
        padding: 4px 0;
    }

    .order-totals p {
        margin: 4px 0;
    }

    .btn-small {
        padding: 4px 8px;
        background: #0077ff;
        color: white;
        border: none;
        border-radius: 6px;
        cursor: pointer;
    }

        .btn-small:hover {
            background: #005fcc;
        }
</style>
