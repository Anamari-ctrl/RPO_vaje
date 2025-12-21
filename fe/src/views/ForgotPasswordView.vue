<template>
  <div class="password-container">
    <div class="password-card">
      <h2>Forgot Password?</h2>
      <p class="subtitle">Enter your email address and we'll send you a link to reset your password.</p>
      
      <form @submit.prevent="handleForgotPassword">
        <div class="form-group">
          <label for="email">Email Address</label>
          <input
            type="email"
            id="email"
            v-model="email"
            required
            placeholder="Enter your email"
            :disabled="isLoading"
          />
        </div>

        <div v-if="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>

        <div v-if="successMessage" class="success-message">
          {{ successMessage }}
        </div>

        <button type="submit" class="btn-primary" :disabled="isLoading">
          {{ isLoading ? 'Sending...' : 'Send Reset Link' }}
        </button>
      </form>

      <div class="back-link">
        <router-link to="/login">← Back to Login</router-link>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import accountService from '@/services/account-service';

export default {
  name: 'ForgotPasswordView',
  setup() {
    const router = useRouter();
    const email = ref('');
    const errorMessage = ref('');
    const successMessage = ref('');
    const isLoading = ref(false);

    const handleForgotPassword = async () => {
      errorMessage.value = '';
      successMessage.value = '';
      isLoading.value = true;

      try {
        await accountService.postForgotPassword(email.value);

        successMessage.value = 'Check your email for the reset link. Redirecting to login...';
        
        setTimeout(() => {
          router.push('/login');
        }, 3000);
      } catch (error) {
        errorMessage.value = error.message || 'Failed to send reset link. Please try again.';
      } finally {
        isLoading.value = false;
      }
    };

    return {
      email,
      errorMessage,
      successMessage,
      isLoading,
      handleForgotPassword
    };
  }
};
</script>

<style scoped>
.password-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: 20px;
}

.password-card {
  background: white;
  padding: 40px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
}

h2 {
  margin-top: 0;
  margin-bottom: 15px;
  text-align: center;
  color: #333;
}

.subtitle {
  text-align: center;
  color: #666;
  font-size: 0.95rem;
  margin-bottom: 25px;
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

.success-message {
  background-color: #efe;
  color: #3c3;
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

.back-link {
  margin-top: 20px;
  text-align: center;
}

.back-link a {
  color: #42b983;
  text-decoration: none;
  font-weight: 600;
}

.back-link a:hover {
  text-decoration: underline;
}
</style>
