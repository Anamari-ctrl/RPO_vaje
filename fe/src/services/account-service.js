import { reactive, readonly } from 'vue';

class AccountService {
  constructor() {
    this.state = reactive({
      isUserLoggedIn: this.hasToken(),
      currentEmail: null,
      currentUserId: null,
      currentFullName: null
    });

    if (this.hasToken()) {
      this.refreshUserData();
    }
  }

  getState() {
    return readonly(this.state);
  }

  async postLogin(loginUser) {
    const response = await fetch('/api/accounts/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(loginUser)
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Login failed');
    }

    return await response.json();
  }


  async postLogout() {
    const headers = this.getHeaderData();
    
    try {
      const response = await fetch('/api/accounts/logout', {
        method: 'GET',
        headers: headers
      });

      if (!response.ok) {
        console.error('Logout API call failed');
      }

      return await response.text();
    } finally {
      this.logout();
    }
  }

  login(token) {
    localStorage.setItem('token', token);
    this.state.isUserLoggedIn = true;
  }

  logout() {
    this.state.currentEmail = null;
    this.state.currentUserId = null;
    this.state.currentFullName = null;
    this.state.isUserLoggedIn = false;

    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('userId');
    localStorage.removeItem('fullName');
  }

  hasToken() {
    return !!localStorage.getItem('token');
  }


  getHeaderData() {
    const headers = {
      'Content-Type': 'application/json'
    };

    const token = localStorage.getItem('token');
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    return headers;
  }

  refreshUserData() {
    this.state.currentEmail = localStorage.getItem('email');
    this.state.currentUserId = localStorage.getItem('userId');
    this.state.currentFullName = localStorage.getItem('fullName');
  }

  saveUserData(email, userId, fullName) {
    localStorage.setItem('email', email);
    localStorage.setItem('userId', userId);
    localStorage.setItem('fullName', fullName);
    
    this.state.currentEmail = email;
    this.state.currentUserId = userId;
    this.state.currentFullName = fullName;
  }
}


export default new AccountService();
