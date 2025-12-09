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

                <table v-if="orders.length > 0" class="orders-table">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Date</th>
                            <th>Status</th>
                            <th>Total</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="o in orders" :key="o.id">
                            <td>#{{ o.id }}</td>
                            <td>{{ new Date(o.createdAt).toLocaleDateString() }}</td>
                            <td>{{ o.status }}</td>
                            <td>{{ o.total }} €</td>
                            <td><button class="btn-small" @click="selectedOrder = o">View</button></td>
                        </tr>
                    </tbody>
                </table>

                <div v-if="selectedOrder" class="order-details">
                    <h4>Order #{{ selectedOrder.id }}</h4>
                    <div v-for="item in selectedOrder.items" :key="item.productId" class="order-item">
                        <strong>{{ item.productName }}</strong><br />
                        Quantity: {{ item.quantity }}<br />
                        Price: {{ item.price }} €
                    </div>
                    <button class="btn-small" @click="selectedOrder = null">Close</button>
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

            const user = ref({
                firstName: "",
                lastName: "",
                email: "",
                address: "",
                city: "",
                country: "",
                postalCode: "",
                phoneNumber: "",
                password: "",
                newPassword: "",
                confirmPassword: ""
            });

            const orders = ref([]);
            const selectedOrder = ref(null);

            const logout = () => {
                accountService.logout();
                router.push("/login");
            };

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

            const saveProfile = async () => {
                try {
                    const headers = accountService.getHeaderData();
                    const response = await fetch("http://localhost:7020/api/v1/users/me", {
                        method: "PUT",
                        headers,
                        body: JSON.stringify(user.value)
                    });
                    if (!response.ok) throw new Error("Failed to save profile");
                    alert("Profile updated successfully!");
                } catch (err) {
                    console.error(err);
                }
            };

            const fetchOrders = async () => {
                try {
                    orders.value = await orderService.getMyOrders();
                } catch (err) {
                    console.error(err);
                }
            };

            watch(activeScreen, (screen) => {
                if (screen === "orders") fetchOrders();
            });

            onMounted(fetchProfile);

            return { user, logout, activeScreen, saveProfile, orders, selectedOrder };
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


    .orders-table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 10px;
    }

        .orders-table th,
        .orders-table td {
            border: 1px solid #ddd;
            padding: 8px;
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
