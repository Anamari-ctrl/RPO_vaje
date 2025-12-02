<template>
    <div class="register-container">
        <div class="register-card">
            <h2>Register</h2>

            <form @submit.prevent="handleRegister">
                <div class="form-group">
                    <label for="fullName">Full Name</label>
                    <input type="text"
                           id="fullName"
                           v-model="registerForm.fullName"
                           required
                           placeholder="Enter your full name"
                           :disabled="isLoading" />
                </div>

                <div class="form-group">
                    <label for="email">Email</label>
                    <input type="email"
                           id="email"
                           v-model="registerForm.email"
                           required
                           placeholder="Enter your email"
                           :disabled="isLoading" />
                </div>

                <div class="form-group">
                    <label for="password">Password</label>
                    <input type="password"
                           id="password"
                           v-model="registerForm.password"
                           required
                           placeholder="Enter your password"
                           :disabled="isLoading" />
                </div>

                <div v-if="errorMessage" class="error-message">
                    {{ errorMessage }}
                </div>

                <button type="submit" class="btn-primary" :disabled="isLoading">
                    {{ isLoading ? 'Registering...' : 'Register' }}
                </button>
            </form>

            <div class="register-link">
                <p>Already have an account? <router-link to="/login">Login here</router-link></p>
            </div>
        </div>
    </div>
</template>

<script>
    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import accountService from '../services/account-service';

    export default {
        name: 'RegisterView',
        setup() {
            const router = useRouter();

            const registerForm = ref({
                fullName: '',
                email: '',
                password: ''
            });

            const errorMessage = ref('');
            const isLoading = ref(false);

            const handleRegister = async () => {
                errorMessage.value = '';
                isLoading.value = true;

                try {
                    const response = await accountService.postRegister({
                        fullName: registerForm.value.fullName,
                        email: registerForm.value.email,
                        password: registerForm.value.password
                    });

                    // After successful register, auto-login if API returns token
                    if (response.token) {
                        accountService.login(response.token);

                        if (response.email && response.userId) {
                            accountService.saveUserData(
                                response.email,
                                response.userId,
                                response.fullName || ''
                            );
                        }

                        router.push('/');
                    } else {
                        // If no token returned, redirect to login
                        router.push('/login');
                    }
                } catch (error) {
                    errorMessage.value = error.message || 'Registration failed. Please try again.';
                } finally {
                    isLoading.value = false;
                }
            };

            return {
                registerForm,
                errorMessage,
                isLoading,
                handleRegister
            };
        }
    };
</script>

<style scoped>
    .register-container {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 80vh;
        padding: 20px;
    }

    .register-card {
        background: white;
        padding: 40px;
        border-radius: 8px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        width: 100%;
        max-width: 400px;
    }

    h2 {
        margin-top: 0;
        margin-bottom: 30px;
        text-align: center;
        color: #333;
    }

    .form-group {
        margin-bottom: 20px;
    }

    label {
        display: block;
        margin-bottom: 5px;
        color: #555;
        font-weight: 500;
    }

    input {
        width: 100%;
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
        box-sizing: border-box;
    }

        input:focus {
            outline: none;
            border-color: #42b983;
        }

        input:disabled {
            background-color: #f5f5f5;
            cursor: not-allowed;
        }

    .error-message {
        background-color: #fee;
        color: #c33;
        padding: 10px;
        border-radius: 4px;
        margin-bottom: 20px;
        font-size: 14px;
    }

    .btn-primary {
        width: 100%;
        padding: 12px;
        background-color: #42b983;
        color: white;
        border: none;
        border-radius: 4px;
        font-size: 16px;
        font-weight: 600;
        cursor: pointer;
        transition: background-color 0.3s;
    }

        .btn-primary:hover:not(:disabled) {
            background-color: #359268;
        }

        .btn-primary:disabled {
            background-color: #999;
            cursor: not-allowed;
        }

    .register-link {
        margin-top: 20px;
        text-align: center;
    }

        .register-link p {
            color: #666;
            font-size: 14px;
        }

        .register-link a {
            color: #42b983;
            text-decoration: none;
            font-weight: 600;
        }

            .register-link a:hover {
                text-decoration: underline;
            }
</style>
