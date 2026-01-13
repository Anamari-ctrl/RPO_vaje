import { reactive, readonly } from 'vue';
const API_BASE_URL = "http://localhost:7020/api/v1/auth";
const USER_API_URL = "http://localhost:7020/api/v1/users/me";

class AccountService {
    constructor() {
        this.state = reactive({
            isUserLoggedIn: this.hasToken(),
            currentEmail: null,
            currentUserId: null,
            firstName: null,
            lastName: null
        });

        if (this.hasToken()) {
            this.refreshUserData();
        }
    }

    getState() {
        return readonly(this.state);
    }

    async postRegister(registerUser) {
        const response = await fetch(`${API_BASE_URL}/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(registerUser)
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Registration failed");
        }

        return await response.json();
    }

    async postLogin(loginUser) {
        // Map payload to backend DTO keys
        const payload = {
            Email: loginUser.email ?? loginUser.Email,
            Password: loginUser.password ?? loginUser.Password
        };

        const response = await fetch(`${API_BASE_URL}/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Login failed");
        }

        const data = await response.json();

        // Save tokens
        this.login(data.Token || data.token, data.RefreshToken || data.refreshToken);

        // Persist token expirations if needed
        if (data.Expiration) localStorage.setItem('tokenExpiration', new Date(data.Expiration).toISOString());
        if (data.RefreshTokenExpirationDateTime) localStorage.setItem('refreshTokenExpiration', new Date(data.RefreshTokenExpirationDateTime).toISOString());

        // Fetch user data
        await this.fetchUserData();

        return data;
    }

    async postForgotPassword(email) {
        const payload = {
            Email: email,
            ClientUri: `${window.location.origin}/reset-password`
        };

        const response = await fetch(`${API_BASE_URL}/forgot-password`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        const data = await response.json().catch(() => null);

        if (!response.ok) {
            const msg = data?.detail || data?.message || data?.title || "Failed to send reset link";
            throw new Error(msg);
        }

        // data is usually a string callback url
        return data;
    }


    async postResetPassword({ token, newPassword, confirmPassword, email = null }) {
        const payload = {
            Token: token,
            NewPassword: newPassword,
            ConfirmPassword: confirmPassword,
            Email: email
        };

        const response = await fetch(`${API_BASE_URL}/reset-password`, {
            method: "POST",
            headers: { "Content-Type": "application/json", ...this.getHeaderData() },
            body: JSON.stringify(payload)
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            // Backend may return { Errors: [...] }
            const message = error.message || (error.Errors ? error.Errors.join(" | ") : "Failed to reset password");
            throw new Error(message);
        }

        return await response.json().catch(() => ({}));
    }

    async postLogout() {
        const headers = this.getHeaderData();

        try {
            await fetch(`${API_BASE_URL}/logout`, {
                method: 'GET',
                headers
            });
        } finally {
            this.logout();
        }
    }

    login(token, refreshToken = null) {
        if (token) localStorage.setItem('token', token);
        if (refreshToken) localStorage.setItem('refreshToken', refreshToken);
        this.state.isUserLoggedIn = true;
    }

    logout() {
        localStorage.clear();
        this.state.isUserLoggedIn = false;
        this.state.currentEmail = null;
        this.state.currentUserId = null;
        this.state.firstName = null;
        this.state.lastName = null;
    }

    hasToken() {
        return !!localStorage.getItem('token');
    }

    getHeaderData() {
        const token = localStorage.getItem('token');
        return {
            'Content-Type': 'application/json',
            ...(token && { 'Authorization': `Bearer ${token}` })
        };
    }

    async postRefreshToken() {
        const token = localStorage.getItem('token');
        const refreshToken = localStorage.getItem('refreshToken');

        if (!token || !refreshToken) {
            throw new Error('No tokens available to refresh');
        }

        const response = await fetch(`${API_BASE_URL}/refresh`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Token: token, RefreshToken: refreshToken })
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Failed to refresh token");
        }

        const data = await response.json();

        // Update tokens
        this.login(data.Token || data.token, data.RefreshToken || data.refreshToken);
        if (data.Expiration) localStorage.setItem('tokenExpiration', new Date(data.Expiration).toISOString());
        if (data.RefreshTokenExpirationDateTime) localStorage.setItem('refreshTokenExpiration', new Date(data.RefreshTokenExpirationDateTime).toISOString());

        return data;
    }

    async fetchUserData() {
        const response = await fetch(USER_API_URL, {
            method: "GET",
            headers: this.getHeaderData()
        });

        if (!response.ok) return;

        const user = await response.json();

        this.saveUserData(user.email, user.userId, user.firstName, user.lastName);
    }

    async getUserById(userId) {
        if (!userId) return null;
        try {
            const response = await fetch(`${USER_API_URL}?userId=${userId}`, {
                method: "GET",
                headers: this.getHeaderData()
            });

            if (!response.ok) return null;

            return await response.json();
        } catch (error) {
            console.error("Error fetching user data", error);
            return null;
        }
    }

    saveUserData(email, userId, firstName, lastName) {
        localStorage.setItem('email', email);
        localStorage.setItem('userId', userId);
        localStorage.setItem('firstName', firstName);
        localStorage.setItem('lastName', lastName);

        this.state.currentEmail = email;
        this.state.currentUserId = userId;
        this.state.firstName = firstName;
        this.state.lastName = lastName;
    }

    refreshUserData() {
        this.state.currentEmail = localStorage.getItem('email');
        this.state.currentUserId = localStorage.getItem('userId');
        this.state.firstName = localStorage.getItem('firstName');
        this.state.lastName = localStorage.getItem('lastName');
    }
}

export default new AccountService();
