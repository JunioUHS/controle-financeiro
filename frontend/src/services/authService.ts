import { apiService } from './apiService';
import { STORAGE_KEYS, API_ENDPOINTS } from '../config/constants';
import type {
    LoginCredentials,
    RegisterCredentials,
    User,
} from '../types/auth';
import type { ApiResponse } from '../types/api';
import type { JWTClaims } from '../types/jwt';

class AuthService {
    async login(credentials: LoginCredentials): Promise<ApiResponse<string>> {
        const response = await apiService.post<string, LoginCredentials>(
            API_ENDPOINTS.AUTH.LOGIN,
            credentials
        );

        if (response.success && response.data) {
            // Armazenar apenas o JWT token - refresh token fica em cookie automático
            this.storeToken(response.data);
        }

        return response;
    }

    async register(credentials: RegisterCredentials): Promise<ApiResponse<void>> {
        const response = await apiService.post<void, RegisterCredentials>(
            API_ENDPOINTS.USER.REGISTER,
            credentials
        );
        return response;
    }

    async refreshToken(): Promise<ApiResponse<string>> {
        const response = await apiService.post<string>(API_ENDPOINTS.AUTH.REFRESH);

        if (response.success && response.data) {
            this.storeToken(response.data);
        }

        return response;
    }

    async logout(): Promise<ApiResponse<void>> {
        const response = await apiService.post<void>(API_ENDPOINTS.AUTH.LOGOUT);

        // Limpar dados locais independente da resposta da API
        this.clearStorage();

        return response;
    }

    async getCurrentUser(): Promise<ApiResponse<User>> {
        const response = await apiService.get<User>(API_ENDPOINTS.USER.CURRENT);
        return response;
    }

    // Métodos de armazenamento
    private storeToken(token: string): void {
        localStorage.setItem(STORAGE_KEYS.TOKEN, token);
    }

    storeUser(user: User): void {
        localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user));
    }

    getStoredToken(): string | null {
        return localStorage.getItem(STORAGE_KEYS.TOKEN);
    }

    getStoredUser(): User | null {
        const userData = localStorage.getItem(STORAGE_KEYS.USER);
        return userData ? JSON.parse(userData) : null;
    }

    clearStorage(): void {
        localStorage.removeItem(STORAGE_KEYS.TOKEN);
        localStorage.removeItem(STORAGE_KEYS.USER);
    }

    isAuthenticated(): boolean {
        return !!this.getStoredToken();
    }

    // Decodificar JWT para obter dados do usuário
    decodeToken(token: string): User | null {
        try {
            const base64Url = token.split('.')[1];
            if (!base64Url) {
                throw new Error('Token JWT inválido');
            }

            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const jsonPayload = decodeURIComponent(
                atob(base64)
                    .split('')
                    .map(function (c) {
                        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
                    })
                    .join('')
            );

            const decoded = JSON.parse(jsonPayload) as JWTClaims;

            // Mapear claims do JWT para User com verificação de tipos
            return {
                id: decoded.sub || decoded.nameid || '',
                userName: decoded.unique_name || decoded.username || '',
                fullName: decoded.given_name || decoded.name || '',
                email: decoded.email
            };
        } catch (error) {
            console.error('Erro ao decodificar token:', error);
            return null;
        }
    }
}

export const authService = new AuthService();