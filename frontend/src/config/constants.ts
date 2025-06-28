export const API_CONFIG = {
    BASE_URL: import.meta.env.VITE_API_URL || 'http://localhost:5097/api',
    TIMEOUT: 100000,
} as const;

export const STORAGE_KEYS = {
    TOKEN: 'auth_token',
    USER: 'user_data',
} as const;

export const ROUTES = {
    LOGIN: '/login',
    REGISTER: '/register',
    DASHBOARD: '/dashboard',
    HOME: '/',
} as const;

export const API_ENDPOINTS = {
    AUTH: {
        LOGIN: '/auth/login',
        REFRESH: '/auth/refresh-token',
        LOGOUT: '/auth/logout',
    },
    USER: {
        REGISTER: '/user/register',
        CURRENT: '/user/current',
    }
} as const;