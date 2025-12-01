import { apiCall } from './apiClient';

// Mock data for development fallback
const mockProjects = [
    {
        id: 1,
        shortName: 'TCVid',
        title: 'Tech Corp Promo Video',
        type: 'Video',
        focus: 'promote tech solutions',
        status: 'In Progress',
        client: 'Steve Bob',
        producer: 'John Doe',
        qtyPerUnit: 1,
        pricePerUnit: 5000,
        editor: 'Jane Smith',
        expenseBudget: 2000,
        expenseSummary: 'travel,food 5 days',
        comments: 'N/A',
        scope: 'promo video for Tech Corp showcasing new product features.',
        startDate: '2024-01-15',
        endDate: '2024-03-15',
    },
    {
        id: 2,
        shortName: 'JP-Photo',
        title: 'Joe Plumber Photo Shoot',
        type: 'Photo Shoot',
        focus: 'photograph end to end plumbing solutions',
        status: 'Completed',
        client: 'Joe Plumber',
        producer: 'John Doe',
        qtyPerUnit: 1,
        pricePerUnit: 1,
        editor: 'Jane Smith',
        expenseBudget: 5000,
        expenseSummary: 'travel,food 3 days',
        comments: 'N/A',
        scope: 'Photo shoot for Joe Plumber\'s new marketing campaign.',
        startDate: '2024-01-15',
        endDate: '2024-03-15',
    },
    {
        id: 3,
        shortName: 'KidsDent',
        title: 'Kids Dental Care Campaign',
        type: 'Video+Photo',
        focus: 'kids dental care',
        status: 'Not Started',
        client: 'Smith Family Dentistry',
        producer: 'John Doe',
        qtyPerUnit: 1,
        pricePerUnit: 1,
        editor: 'Jane Smith',
        expenseBudget: 5000,
        expenseSummary: 'food, studio teacher 3 days',
        comments: 'N/A',
        scope: 'Video and photo campaign for Kids Dental Care.',
        startDate: '2024-01-15',
        endDate: '2024-03-15',
    }
];

/**
 * Project API service
 * Handles all project-related API calls
 */
export const projectApi = {
    /**
     * Get all projects
     * @returns {Promise<Array>} List of projects
     */
    getAll: async () => {
        try {
            return await apiCall('/Project/projects');
        } catch (error) {
            console.error('Failed to fetch projects:', error);
            return mockProjects; // Fallback to mock data
        }
    },

    /**
     * Get a single project by ID
     * @param {number|string} id - Project ID
     * @returns {Promise<Object>} Project details
     */
    getById: async (id) => {
        try {
            return await apiCall(`/Project/project/${id}`);
        } catch (error) {
            console.error('Failed to fetch project:', error);
            throw error;
        }
    },

    /**
     * Create a new project
     * @param {Object} projectData - Project data
     * @returns {Promise<Object>} Created project
     */
    create: async (projectData) => {
        try {
            return await apiCall('/Project/project/new', {
                method: 'POST',
                body: JSON.stringify(projectData),
            });
        } catch (error) {
            console.error('Failed to create project:', error);
            throw error;
        }
    },

    /**
     * Update an existing project
     * @param {number|string} id - Project ID
     * @param {Object} projectData - Updated project data
     * @returns {Promise<Object>} Updated project
     */
    update: async (id, projectData) => {
        try {
            return await apiCall(`/Project/project/${id}/update`, {
                method: 'PUT',
                body: JSON.stringify(projectData),
            });
        } catch (error) {
            console.error('Failed to update project:', error);
            throw error;
        }
    },

    /**
     * Delete a project
     * @param {number|string} id - Project ID
     * @returns {Promise<void>}
     */
    delete: async (id) => {
        try {
            await apiCall(`/Project/project/${id}`, { method: 'DELETE' });
        } catch (error) {
            console.error('Failed to delete project:', error);
            throw error;
        }
    }

    /**
     * Update project milestone
     * @param {number|string} projectId - Project ID
     * @param {number|string} milestoneId - Milestone ID
     * @returns {Promise<Object>} Updated project
     */
    ,
    updateMilestone: async (projectId, milestoneId) => {
        try {
            return await apiCall(`/Project/project/${projectId}/milestone/${milestoneId}`, {
                method: 'PUT',
            });
        } catch (error) {
            console.error('Failed to update project milestone:', error);
            throw error;
        }
    }
};
