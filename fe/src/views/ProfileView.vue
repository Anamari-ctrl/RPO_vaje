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
                    Hello {{ user.name || "User" }}
                    (not you?<span class="link" @click="logout"> Logout</span>)
                </p>

                <p>
                    In your account dashboard you can <span class="link" @click="activeScreen = 'orders'">view your recent orders</span> and <span class="link" @click="activeScreen = 'settings'">edit your account information</span>
                </p>
            </div>

            <!-- Account Settings -->
            <div v-if="activeScreen === 'settings'">
                <h3>Account Settings</h3>

                <form @submit.prevent="saveProfile">
                    <div class="form-group">
                        <label>Name</label>
                        <input v-model="user.name" type="text" required />
                    </div>
                    <div class="form-group">
                        <label>Lastname</label>
                        <input v-model="user.lastname" type="text" required />
                    </div>

                    <div class="form-group">
                        <label>Email</label>
                        <input v-model="user.email" type="email" required />
                    </div>


                    <h3>Change password</h3>

                    <div class="form-group">
                        <label>Current Password</label>
                        <input v-model="user.password" type="password" placeholder="Your password" />
                    </div>
                    <div class="form-group">
                        <label>New Password</label>
                        <input v-model="user.newPassword" type="password" placeholder="New password" />
                    </div>
                    <div class="form-group">
                        <label>Confirm Password</label>
                        <input v-model="user.confirmPassword" type="password" placeholder="Re-enter your password" />
                    </div>

                    <button type="submit" class="btn-primary">Save Changes</button>
                </form>
            </div>




            <div v-if="activeScreen === 'orders'">
                <h3>Order History</h3>

                <!-- no orders -->
                <p v-if="orders.length === 0">You do not have any orders yet.</p>

                <!-- orders tabela -->
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
                            <td>
                                <button class="btn-small" @click="selectedOrder = o">
                                    View
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <!-- ORDER DETAILS -->
                <div v-if="selectedOrder" class="order-details">
                    <h4>Order #{{ selectedOrder.id }}</h4>

                    <div v-for="item in selectedOrder.items" :key="item.bookId" class="order-item">
                        <strong>{{ item.bookTitle }}</strong><br />
                        Quantity: {{ item.quantity }}<br />
                        Price: {{ item.price }} €
                    </div>

                    <button class="btn-small" @click="selectedOrder = null">
                        Close
                    </button>
                </div>
            </div>


        </div>

    </div>
</template>

<script>
    import { ref, watch } from "vue";
    import { useRouter } from "vue-router";
    import accountService from "../services/account-service";
    import orderService from "../services/order-service";

    export default {
        name: "ProfileView",
        setup() {
            const router = useRouter();

            const activeScreen = ref("dashboard");

            const user = ref({
                name: "",
                lastname: "",
                email: "",
            });

            const orders = ref([]);
            const selectedOrder = ref(null);

            const logout = () => {
                accountService.logout();
                router.push("/login");
            };

            const fetchOrders = async () => {
                try {
                    const result = await orderService.getMyOrders();
                    orders.value = result;
                } catch (err) {
                    console.error("Order load failed:", err);
                }
            };

            watch(activeScreen, (screen) => {
                if (screen === "orders") {
                    fetchOrders();
                }
            });

            return {
                user,
                logout,
                activeScreen,
                orders,
                selectedOrder
            };
        },
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

    /* clickable dashboard links */
    .dash-links {
        margin-top: 15px;
    }

        .dash-links li {
            cursor: pointer;
            margin-bottom: 8px;
            color: #0077ff;
        }

            .dash-links li:hover {
                text-decoration: underline;
            }

    /* clickable "Logout" inside text */
    .link {
        color: #0077ff;
        cursor: pointer;
    }

        .link:hover {
            text-decoration: underline;
        }

    /* form styling */
    .form-group {
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
</style>
