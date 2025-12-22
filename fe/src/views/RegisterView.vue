<template>
    <div class="register-container">
        <div class="register-card">
            <h2>Register</h2>

            <form @submit.prevent="handleRegister">
                <div class="form-group">
                    <label for="firstName">First Name</label>
                    <input id="firstName"
                           type="text"
                           v-model="registerForm.firstName"
                           placeholder="Your first name, e.g. Jane"
                           required
                           :disabled="isLoading" />
                </div>

                <div class="form-group">
                    <label for="lastName">Last Name</label>
                    <input id="lastName"
                           type="text"
                           v-model="registerForm.lastName"
                           placeholder="Your last name, e.g. Doe"
                           required
                           :disabled="isLoading" />
                </div>

                <div class="form-group">
                    <label for="email">Email</label>
                    <input id="email"
                           type="email"
                           v-model="registerForm.email"
                           placeholder="janedoe@mail.com"
                           required
                           :disabled="isLoading" />
                </div>

                <div class="form-group">
                    <label for="password">Password</label>
                    <input id="password"
                           type="password"
                           v-model="registerForm.password"
                           placeholder="Your password"
                           required
                           :disabled="isLoading" />
                </div>

                <div class="form-group">
                    <label for="confirmPassword">Confirm Password</label>
                    <input id="confirmPassword"
                           type="password"
                           v-model="registerForm.confirmPassword"
                           placeholder="Re-enter your password"
                           required
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
                <p>
                    Already have an account?
                    <router-link to="/login">Login here</router-link>
                </p>
            </div>
        </div>
    </div>
</template>

<script>
    import { ref } from "vue";
    import { useRouter } from "vue-router";
    import accountService from "../services/account-service";

    export default {
        name: "RegisterView",
        setup() {
            const router = useRouter();

            const registerForm = ref({
                firstName: "",
                lastName: "",
                email: "",
                password: "",
                confirmPassword: ""
            });

            const errorMessage = ref("");
            const isLoading = ref(false);

            const handleRegister = async () => {
                errorMessage.value = "";

                if (registerForm.value.password !== registerForm.value.confirmPassword) {
                    errorMessage.value = "Passwords do not match.";
                    return;
                }

                isLoading.value = true;

                try {
                    const response = await accountService.postRegister({
                        // ⬅️ POMEMBNO: PascalCase za backend
                        FirstName: registerForm.value.firstName,
                        LastName: registerForm.value.lastName,
                        Email: registerForm.value.email,
                        Password: registerForm.value.password,
                        ConfirmPassword: registerForm.value.confirmPassword
                    });

                    if (response?.token) {
                        accountService.login(response.token);

                        accountService.saveUserData(
                            response.email,
                            response.userId,
                            `${response.firstName} ${response.lastName}`
                        );

                        router.push("/");
                    } else {
                        router.push("/login");
                    }
                } catch (err) {
                    errorMessage.value =
                        err?.message || "Registration failed. Please try again.";
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
        font-weight: 500;
        color: #555;
    }

    input {
        width: 100%;
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
    }

        input:focus {
            outline: none;
            border-color: #42b983;
        }

        input:disabled {
            background-color: #f5f5f5;
        }

    .error-message {
        background-color: #fee;
        color: #c33;
        padding: 10px;
        border-radius: 4px;
        margin-bottom: 20px;
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

        .register-link a {
            color: #42b983;
            font-weight: 600;
            text-decoration: none;
        }

            .register-link a:hover {
                text-decoration: underline;
            }
</style>
