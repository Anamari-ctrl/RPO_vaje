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

        const response = await fetch(`${API_BASE_URL}/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginUser)
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({}));
            throw new Error(error.message || "Login failed");
        }

        const data = await response.json();

        // Save token
        this.login(data.token);

        // Fetch user data
        await this.fetchUserData();

        return data;
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

    login(token) {
        localStorage.setItem('token', token);
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

    async fetchUserData() {
        const response = await fetch(USER_API_URL, {
            method: "GET",
            headers: this.getHeaderData()
        });

        if (!response.ok) return;

        const user = await response.json();

        this.saveUserData(user.email, user.userId, user.firstName, user.lastName);
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
