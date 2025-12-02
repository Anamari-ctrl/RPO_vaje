<template>
    <div class="profile-container">
        <!-- Profile card -->
        <div class="profile-card">
            <h2>Your Profile</h2>
            <p class="greeting">Hello <span>{{ user.fullName }}</span>!</p>

            <div class="info-row">
                <strong>Full name:</strong>
                <span>{{ user.fullName }}</span>
            </div>

            <div class="info-row">
                <strong>Email:</strong>
                <span>{{ user.email }}</span>
            </div>

            <div class="info-row">
                <strong>User ID:</strong>
                <span>{{ user.userId }}</span>
            </div>

            <button class="logout-btn" @click="logout">Logout</button>
        </div>

        <!-- Order history card -->
        <div class="order-card">
            <h2>Order History</h2>

            <div v-if="user.orderHistory && user.orderHistory.length" class="orders-list">
                <div class="order-item"
                     v-for="(order, index) in user.orderHistory"
                     :key="index">
                    <strong>Order #{{ index + 1 }}</strong>
                    <p>{{ order }}</p>
                </div>
            </div>

            <p v-else class="empty-text">You have no past orders.</p>
        </div>
    </div>
</template>

<script>
    import { ref, onMounted } from "vue";
    import { useRouter } from "vue-router";
    import accountService from "../services/account-service";

    export default {
        name: "ProfileView",
        setup() {
            const router = useRouter();

            const user = ref({
                fullName: "",
                email: "",
                userId: "",
                orderHistory: [] 
            });

            onMounted(() => {
                if (!accountService.getToken()) {
                    router.push("/login");
                    return;
                }

                const stored = accountService.getUserData();
                if (stored) {
                    user.value = stored;
                }
            });

            const logout = () => {
                accountService.logout();
                router.push("/login");
            };

            return { user, logout };
        }
    };
</script>

<style scoped>
    .profile-container {
        display: flex;
        gap: 30px;
        justify-content: center;
        align-items: flex-start;
        padding: 40px 20px;
        flex-wrap: wrap;
    }

    .profile-card,
    .order-card {
        background: white;
        padding: 35px;
        width: 100%;
        max-width: 420px;
        border-radius: 12px;
        box-shadow: 0 2px 10px rgba(0,0,0,0.08);
    }

    h2 {
        text-align: center;
        margin-bottom: 20px;
        color: #333;
    }

    .greeting {
        text-align: center;
        margin-bottom: 25px;
        font-size: 1.1rem;
    }

        .greeting span {
            font-weight: 600;
        }

    .info-row {
        display: flex;
        justify-content: space-between;
        padding: 10px 0;
        border-bottom: 1px solid #eee;
        font-size: 16px;
    }

    .logout-btn {
        margin-top: 30px;
        width: 100%;
        padding: 12px;
        background-color: #c0392b;
        color: white;
        border: none;
        border-radius: 8px;
        font-weight: 600;
        cursor: pointer;
        transition: background 0.25s;
    }

        .logout-btn:hover {
            background-color: #a83224;
        }

    .orders-list {
        display: flex;
        flex-direction: column;
        gap: 15px;
    }

    .order-item {
        padding: 12px;
        border-radius: 8px;
        background: #f8f8f8;
    }

    .empty-text {
        text-align: center;
        color: #888;
        font-size: 0.95rem;
        margin-top: 10px;
    }
</style>
