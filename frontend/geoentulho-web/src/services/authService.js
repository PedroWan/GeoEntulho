import api from './api';

export const authService = {
  register: async (email, password, name, type = 'citizen') => {
    const response = await api.post('/api/auth/register', {
      email,
      password,
      name,
      type,
    });
    return response.data;
  },

  login: async (email, password) => {
    const response = await api.post('/api/auth/login', {
      email,
      password,
    });
    if (response.data && response.data.token) {
      localStorage.setItem('accessToken', response.data.token);
      // Armazenar apenas o objeto user
      const userData = response.data.user || response.data;
      localStorage.setItem('user', JSON.stringify(userData));
    }
    return response.data.user || response.data;
  },

  logout: () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('user');
  },

  getCurrentUser: () => {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  },

  isAuthenticated: () => {
    return !!localStorage.getItem('accessToken');
  },
};
