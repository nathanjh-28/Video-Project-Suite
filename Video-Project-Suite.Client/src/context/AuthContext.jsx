import React, { createContext, useState, useContext, useEffect } from 'react';
import { userApi } from '../services/api';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [loading, setLoading] = useState(true);

    const checkAuth = async () => {
        setLoading(true);
        const loggedIn = await userApi.isLoggedIn();
        setIsLoggedIn(loggedIn);
        setLoading(false);
    };

    useEffect(() => {
        checkAuth();
    }, []);

    const login = async (credentials) => {
        await userApi.login(credentials);
        await checkAuth(); // Refresh auth state after login
    };

    const logout = async () => {
        await userApi.logout();
        setIsLoggedIn(false);
    };

    const value = {
        isLoggedIn,
        loading,
        login,
        logout,
        checkAuth, // Expose in case you need to manually refresh
    };

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
