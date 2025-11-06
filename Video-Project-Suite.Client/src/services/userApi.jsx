import { apiCall, apiCallNoThrow, API_BASE_URL } from './apiClient';

/**
 * User API service
 * Handles all user and authentication-related API calls
 */
export const userApi = {
    /**
     * Check if user is currently logged in
     * @returns {Promise<boolean>} True if logged in, false otherwise
     */
    isLoggedIn: async () => {
        try {
            // Use fetch directly to check status code without throwing on 401
            const response = await fetch(`${API_BASE_URL}/Test/secure`, {
                headers: {
                    'Content-Type': 'application/json',
                },
                credentials: 'include', // Important for sending cookies
            });

            // Return true only if we get a successful response (200-299)
            return response.ok;
        } catch (error) {
            console.error('Failed to check login status:', error);
            return false;
        }
    },

    /**
     * Get all users
     * @returns {Promise<Array>} List of users
     */
    getAll: async () => {
        try {
            return await apiCall('/Auth/users');
        } catch (error) {
            console.error('Failed to fetch users:', error);
            throw error;
        }
    },

    /**
     * Get a single user by ID
     * @param {number|string} id - User ID
     * @returns {Promise<Object>} User details
     */
    getById: async (id) => {
        try {
            return await apiCall(`/Auth/user/${id}`);
        } catch (error) {
            console.error('Failed to fetch user:', error);
            throw error;
        }
    },

    /**
     * Update a user
     * @param {number|string} id - User ID
     * @param {Object} userData - Updated user data
     * @returns {Promise<Object>} Updated user
     */
    update: async (id, userData) => {
        try {
            return await apiCall(`/Auth/update/${id}`, {
                method: 'PUT',
                body: JSON.stringify(userData),
            });
        } catch (error) {
            console.error('Failed to update user:', error);
            throw error;
        }
    },

    /**
     * Delete a user
     * @param {number|string} id - User ID
     * @returns {Promise<void>}
     */
    delete: async (id) => {
        try {
            await apiCall(`/Auth/delete/${id}`, { method: 'DELETE' });
        } catch (error) {
            console.error('Failed to delete user:', error);
            throw error;
        }
    },

    /**
     * Register a new user
     * @param {Object} userData - User registration data
     * @returns {Promise<Object>} Created user
     */
    register: async (userData) => {
        try {
            return await apiCall('/Auth/register', {
                method: 'POST',
                body: JSON.stringify(userData),
            });
        } catch (error) {
            console.error('Failed to register user:', error);
            throw error;
        }
    },

    /**
     * Login user
     * @param {Object} credentials - Login credentials (username, password)
     * @returns {Promise<Object>} Login response
     */
    login: async (credentials) => {
        try {
            return await apiCall('/Auth/login', {
                method: 'POST',
                body: JSON.stringify(credentials),
            });
        } catch (error) {
            console.error('Failed to login user:', error);
            throw error;
        }
    },

    /**
     * Logout current user
     * @returns {Promise<Object>} Logout response
     */
    logout: async () => {
        try {
            return await apiCall('/Auth/logout', {
                method: 'POST',
            });
        } catch (error) {
            console.error('Failed to logout user:', error);
            throw error;
        }
    },

    /**
     * Change user password
     * @param {Object} passwordData - Password change data (currentPassword, newPassword)
     * @returns {Promise<Object>} Success response
     */
    changePassword: async (passwordData) => {
        try {
            return await apiCall(`/Auth/change-password`, {
                method: 'POST',
                body: JSON.stringify(passwordData),
            });
        } catch (error) {
            console.error('Failed to change password:', error);
            throw error;
        }
    },

    /**
     * Change user role (admin function)
     * @param {Object} roleData - Role change data (userId, newRole)
     * @returns {Promise<Object>} Updated user
     */
    changeUserRole: async (roleData) => {
        try {
            return await apiCall(`/Auth/alter-user-role`, {
                method: 'POST',
                body: JSON.stringify(roleData),
            });
        } catch (error) {
            console.error('Failed to change user role:', error);
            throw error;
        }
    }
};
