<template>
  <div class="password-container">
    <div class="password-card">
      <h2>Reset password</h2>
      <p class="subtitle">Enter a new password below to change your password.</p>
      
      <form @submit.prevent="handleResetPassword">
        <div class="form-group">
          <label for="password">New password</label>
          <input
            type="password"
            id="password"
            v-model="formData.password"
            required
            placeholder=""
            :disabled="isLoading"
          />
        </div>

        <div class="form-group">
          <label for="confirmPassword">Confirm password</label>
          <input
            type="password"
            id="confirmPassword"
            v-model="formData.confirmPassword"
            required
            placeholder=""
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
          {{ isLoading ? 'Resetting...' : 'Reset Password' }}
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
import { useRouter, useRoute } from 'vue-router';
import accountService from '@/services/account-service';

export default {
  name: 'ResetPasswordView',
  setup() {
    const router = useRouter();
    const route = useRoute();
    const token = route.params.token;

    const formData = ref({
      password: '',
      confirmPassword: ''
    });
    const errorMessage = ref('');
    const successMessage = ref('');
    const isLoading = ref(false);

    const handleResetPassword = async () => {
      errorMessage.value = '';
      successMessage.value = '';

      // Validate passwords match
      if (formData.value.password !== formData.value.confirmPassword) {
        errorMessage.value = 'Passwords do not match';
        return;
      }

      // Validate password length
      if (formData.value.password.length < 6) {
        errorMessage.value = 'Password must be at least 6 characters';
        return;
      }

      isLoading.value = true;

      try {
        await accountService.postResetPassword({
          token,
          newPassword: formData.value.password,
          confirmPassword: formData.value.confirmPassword
        });

        successMessage.value = 'Password reset successfully! Redirecting to login...';

        setTimeout(() => {
          router.push('/login');
        }, 2000);
      } catch (error) {
        errorMessage.value = error.message || 'Failed to reset password. Please try again.';
      } finally {
        isLoading.value = false;
      }
    };

    return {
      formData,
      errorMessage,
      successMessage,
      isLoading,
      handleResetPassword
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
        background: transparent;
    }

.password-card {
  background: white;
  padding: 60px 50px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 450px;
}

h2 {
  margin-top: 0;
  margin-bottom: 12px;
  color: #333;
  font-size: 1.8rem;
  font-weight: 600;
}

.subtitle {
  color: #666;
  font-size: 0.95rem;
  margin-bottom: 35px;
  line-height: 1.5;
}

.form-group {
  margin-bottom: 25px;
}

label {
  display: block;
  margin-bottom: 8px;
  color: #333;
  font-weight: 500;
  font-size: 0.95rem;
}

input {
  width: 100%;
  padding: 12px 14px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 15px;
  box-sizing: border-box;
  font-family: inherit;
}

input:focus {
  outline: none;
  border-color: #42b983;
  box-shadow: 0 0 0 3px rgba(66, 185, 131, 0.1);
}

input:disabled {
  background-color: #f5f5f5;
  cursor: not-allowed;
}

.error-message {
  background-color: #fee;
  color: #c33;
  padding: 12px;
  border-radius: 4px;
  margin-bottom: 20px;
  font-size: 13px;
}

.success-message {
  background-color: #efe;
  color: #3c3;
  padding: 12px;
  border-radius: 4px;
  margin-bottom: 20px;
  font-size: 13px;
}

.btn-primary {
  width: 100%;
  padding: 12px;
  background-color: #42b983;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.3s;
  margin-top: 10px;
}

.btn-primary:hover:not(:disabled) {
  background-color: #359268;
}

.btn-primary:disabled {
  background-color: #999;
  cursor: not-allowed;
}

.back-link {
  margin-top: 25px;
  text-align: center;
}

.back-link a {
  color: #42b983;
  text-decoration: none;
  font-weight: 500;
  font-size: 14px;
}

.back-link a:hover {
  text-decoration: underline;
}
</style>
