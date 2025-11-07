// API client configuration and shared utilities
const API_BASE_URL = import.meta.env.VITE_API_URL || '/api';

console.log("API_BASE_URL:", API_BASE_URL);
console.log("VITE_API_URL:", import.meta.env.VITE_API_URL);

/**
 * Generic API call wrapper with error handling
 * Throws errors for non-2xx responses
 */
export const apiCall = async (endpoint, options = {}) => {
    console.log(`API Call: ${API_BASE_URL}${endpoint}`, options);
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            credentials: 'include', // Include cookies in requests
            headers: {
                'Content-Type': 'application/json',
                ...options.headers,
            },
            ...options,
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status} text: ${response.statusText}`);

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

/**
 * API call that doesn't throw on non-2xx responses
 * Useful for auth checks and other status-based logic
 */
export const apiCallNoThrow = async (endpoint, options = {}) => {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                ...options.headers,
            },
            ...options,
        });

        return {
            ok: response.ok,
            status: response.status,
            data: response.ok && response.headers.get('content-type')?.includes('application/json')
                ? await response.json()
                : null
        };
    } catch (error) {
        console.error('API call failed:', error);
        return {
            ok: false,
            status: 0,
            data: null,
            error: error.message
        };
    }
};

export { API_BASE_URL };
