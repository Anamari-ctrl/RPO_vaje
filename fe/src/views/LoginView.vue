<template>
  <div class="login-container">
      <div class="login-card">
          <h2>Login</h2>

          <form @submit.prevent="handleLogin">
              <div class="form-group">
                  <label for="email">Email</label>
                  <input type="email"
                         id="email"
                         v-model="loginForm.email"
                         required
                         placeholder="Enter your email"
                         :disabled="isLoading" />
              </div>

              <div class="form-group">
                  <label for="password">Password</label>
                  <input type="password"
                         id="password"
                         v-model="loginForm.password"
                         required
                         placeholder="Enter your password"
                         :disabled="isLoading" />
              </div>

              <div v-if="errorMessage" class="error-message">
                  {{ errorMessage }}
              </div>

              <button type="submit" class="btn-primary" :disabled="isLoading">
                  {{ isLoading ? 'Logging in...' : 'Login' }}
              </button>
          </form>

          <div class="password-recovery">
              <router-link to="/forgot-password">Forgot your password?</router-link>
          </div>

          <div class="register-link">
              <p>Don't have an account? <router-link to="/register">Register here</router-link></p>
          </div>

          <div class="guest-link">
              <router-link to="/" class="btn-guest">Continue as Guest</router-link>
          </div>


      </div>
  </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import accountService from '../services/account-service';
import { LoginUser } from '../models/login-user';

export default {
  name: 'LoginView',
  setup() {
    const router = useRouter();
    const loginForm = ref({
      email: '',
      password: ''
    });
    const errorMessage = ref('');
    const isLoading = ref(false);

    const handleLogin = async () => {
      errorMessage.value = '';
      isLoading.value = true;

      try {
        const loginUser = new LoginUser(
          loginForm.value.email,
          loginForm.value.password
        );

        const response = await accountService.postLogin(loginUser);

        // Assuming the response contains token and user data
        if (response.token) {
          accountService.login(response.token);
          
          // Save user data if provided
          if (response.email && response.userId) {
            accountService.saveUserData(
              response.email,
              response.userId,
              response.fullName || ''
            );
          }

          // Redirect to home page
          router.push('/');
        }
      } catch (error) {
        errorMessage.value = error.message || 'Login failed. Please try again.';
      } finally {
        isLoading.value = false;
      }
    };

    return {
      loginForm,
      errorMessage,
      isLoading,
      handleLogin
    };
  }
};
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: 20px;
}

.login-card {
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

.password-recovery {
  margin-top: 12px;
  text-align: center;
}

.password-recovery a {
  color: #42b983;
  text-decoration: none;
  font-size: 14px;
  font-weight: 500;
}

.password-recovery a:hover {
  text-decoration: underline;
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
    .btn-guest {
        width: 100%;
        padding: 8px;
        margin-top: 5px;
        background-color: #f0f0f0;
        color: #333;
        border: 1px solid #ccc;
        border-radius: 8px;
        font-size: 16px;
        font-weight: 600;
        text-align: center;
        display: inline-block;
        cursor: pointer;
        transition: all 0.3s ease;
        box-shadow: 0 2px 5px rgba(0,0,0,0.08);
        text-decoration: none;
    }

        .btn-guest:hover {
            background-color: #e0e0e0;
            box-shadow: 0 4px 8px rgba(0,0,0,0.12);
        }


</style>
