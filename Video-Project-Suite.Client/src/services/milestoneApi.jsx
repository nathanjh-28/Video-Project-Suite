import { apiCall } from './apiClient';

const mockMilestones = [
    { id: 1, name: "development" },
    { id: 2, name: "preproduction" },
    { id: 3, name: "production" },
    { id: 4, name: "postproduction" },
    { id: 5, name: "invoiced" },
    { id: 6, name: "completed" },
    { id: 7, name: "milestone 7" },
    { id: 8, name: "milestone 8" },
    { id: 9, name: "milestone 9" },
    { id: 10, name: "milestone 10" }
]


/**
 * Milestone API service
 * Handles all milestone-related API calls
 */
export const milestoneApi = {
    /**
     * Get all Milestones
     * @returns {Promise<Array>} List of milestones
     */
    getAll: async () => {
        try {
            return await apiCall('/Milestone');
        } catch (error) {
            console.error('Failed to fetch projects:', error);
            return mockMilestones; // Fallback to mock data
        }
    },

    /**
     * Get a single project by ID
     * @param {number|string} id - Project ID
     * @returns {Promise<Object>} Project details
     */
    getById: async (id) => {
        try {
            return await apiCall(`/Milestone/${id}`);
        } catch (error) {
            console.error('Failed to fetch milestone:', error);
            throw error;
        }
    },

    /**
     * Create a new Milestone
     * @param {Object} milestoneData - Milestone data
     * @returns {Promise<Object>} Created milestone
     */
    create: async (milestoneData) => {
        try {
            return await apiCall('/Milestone', {
                method: 'POST',
                body: JSON.stringify(milestoneData),
            });
        } catch (error) {
            console.error('Failed to create Milestone:', error);
            throw error;
        }
    },

    /**
     * Update an existing Milestone
     * @param {number|string} id - Milestone ID
     * @param {Object} milestoneData - Updated milestone data
     * @returns {Promise<Object>} Updated milestone
     */
    update: async (id, milestoneData) => {
        try {
            return await apiCall(`/Milestone/${id}`, {
                method: 'PUT',
                body: JSON.stringify(milestoneData),
            });
        } catch (error) {
            console.error('Failed to update Milestone:', error);
            throw error;
        }
    },

    /**
     * Delete a Milestone
     * @param {number|string} id - Milestone ID
     * @returns {Promise<void>}
     */
    delete: async (id) => {
        try {
            await apiCall(`/Milestone/${id}`, { method: 'DELETE' });
        } catch (error) {
            console.error('Failed to delete Milestone:', error);
            throw error;
        }
    },

    changePosition: async (id, newPosition) => {
        try {
            return await apiCall(`/Milestone/${id}/position`, {
                method: 'PUT',
                body: JSON.stringify({ position: newPosition }),
            });
        } catch (error) {
            console.error('Failed to change Milestone position:', error);
            throw error;
        }
    }
};
