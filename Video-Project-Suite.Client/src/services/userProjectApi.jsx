import { apiCall } from './apiClient';

/**
 * UserProject API service
 * Handles all user-project-related API calls
 */

export const userProjectApi = {
    /**
     * Get all user projects
     * @returns {Promise<Array>} List of user projects
     */
    getAllUserProjectAssignments: async () => {
        try {
            return await apiCall('/UserProject/all-user-projects');
        } catch (error) {
            console.error('Failed to fetch user project assignments:', error);
            return []; // Fallback to empty array
        }
    },

    /**
     * Get user project assignment by ID
     * @param {number|string} id - User project assignment ID
     * @returns {Promise<Object>} User project assignment details
     */

    getUserProjectAssignmentById: async (id) => {
        try {
            return await apiCall(`/UserProject/${id}`);
        } catch (error) {
            console.error('Failed to fetch user project assignment:', error);
            throw error;
        }
    },

    /**
     * Update an existing user project assignment
     * @param {number|string} id - User project assignment ID
     * @param {Object} data - Updated user project assignment data
     * @returns {Promise<Object>} Updated user project assignment
     */
    updateUserProjectAssignment: async (id, data) => {
        try {
            return await apiCall(`/UserProject/${id}`, {
                method: 'PUT',
                body: JSON.stringify(data)
            });
        } catch (error) {
            console.error('Failed to update user project assignment:', error);
            throw error;
        }
    },

    /**
     * Delete a project
     * @param {number|string} id - Project ID
     * @returns {Promise<void>}
     */
    deleteUserProjectAssignment: async (id) => {
        try {
            await apiCall(`/UserProject/${id}`, { method: 'DELETE' });
        } catch (error) {
            console.error('Failed to delete user project assignment:', error);
            throw error;
        }
    },

    // create user project assignment

    createUserProjectAssignment: async (data) => {
        try {
            return await apiCall('/UserProject/new-user-project', {
                method: 'POST',
                body: JSON.stringify(data),
            });
        } catch (error) {
            console.error('Failed to create user project assignment:', error);
            throw error;
        }
    },

    // get all user project assignments for a specific user

    getUserProjectAssignmentsByUserId: async (userId) => {
        try {
            return await apiCall(`/UserProject/user/${userId}`, { method: 'GET' });
        } catch (error) {
            console.error('Failed to fetch user project assignments by user ID:', error);
            throw error;
        }
    },

    // get all user project assignments for a specific project

    getUserProjectAssignmentsByProjectId: async (projectId) => {
        try {
            return await apiCall(`/UserProject/project/${projectId}`, { method: 'GET' });
        } catch (error) {
            console.error('Failed to fetch user project assignments by project ID:', error);
            throw error;
        }
    },

}
