// src/services/api.js
// const API_BASE_URL = '/api'; // Replace with your API URL
const API_BASE_URL = import.meta.env.VITE_API_URL || '/api';

console.log("API_BASE_URL:", API_BASE_URL);
console.log("VITE_API_URL:", import.meta.env.VITE_API_URL);

// Mock data for development
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

// API helper function
const apiCall = async (endpoint, options = {}) => {
    console.log(`API Call: ${API_BASE_URL}${endpoint}`, options);
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            headers: {
                'Content-Type': 'application/json',
                ...options.headers,
            },
            ...options,
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        // Only parse JSON if there's content
        const contentType = response.headers.get('content-type');
        if (contentType && contentType.includes('application/json')) {
            return await response.json();
        } else {
            return null;
        }
    } catch (error) {
        console.error('API call failed:', error);
        throw error;
    }
};

// Project API
export const projectApi = {
    getAll: async () => {
        try {
            // Replace with actual API call
            return await apiCall('/Project/projects')
            // return mockProjects; // Using mock data for now
        } catch (error) {
            console.error('Failed to fetch projects:', error);
            return mockProjects; // Fallback to mock data
        }
    },

    getById: async (id) => {
        try {
            return await apiCall(`/Project/project/${id}`);
            // return mockProjects.find(p => p.id === parseInt(id));
        } catch (error) {
            console.error('Failed to fetch project:', error);
            throw error;
        }
    },

    create: async (projectData) => {
        try {
            return await apiCall('/Project/project/new', {
                method: 'POST',
                body: JSON.stringify(projectData),
            });

            // // Mock implementation
            // const newProject = {
            //     id: Date.now(),
            //     created: new Date().toISOString().split('T')[0],
            //     ...projectData
            // };
            // mockProjects.push(newProject);
            // return newProject;
        } catch (error) {
            console.error('Failed to create project:', error);
            throw error;
        }
    },

    update: async (id, projectData) => {
        try {
            return await apiCall(`/Project/project/${id}/update`, {
                method: 'PUT',
                body: JSON.stringify(projectData),
            });

            // Mock implementation
            // const index = mockProjects.findIndex(p => p.id === parseInt(id));
            // if (index !== -1) {
            //     mockProjects[index] = { ...mockProjects[index], ...projectData };
            //     return mockProjects[index];
            // }
        } catch (error) {
            console.error('Failed to update project:', error);
            throw error;
        }
    },

    delete: async (id) => {
        try {
            await apiCall(`/Project/project/${id}`, { method: 'DELETE' });

            // // Mock implementation
            // const index = mockProjects.findIndex(p => p.id === parseInt(id));
            // if (index !== -1) {
            //     mockProjects.splice(index, 1);
            // }
        } catch (error) {
            console.error('Failed to delete project:', error);
            throw error;
        }
    }
};
