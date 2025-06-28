import { API_CONFIG, STORAGE_KEYS } from '../config/constants';
import type { ApiResponse } from '../types/api';

class ApiService {
    private baseURL: string;
    private timeout: number;

    constructor() {
        this.baseURL = API_CONFIG.BASE_URL;
        this.timeout = API_CONFIG.TIMEOUT;
    }

    private async request<T>(
        endpoint: string,
        options: RequestInit = {}
    ): Promise<ApiResponse<T>> {
        const url = `${this.baseURL}${endpoint}`;

        const config: RequestInit = {
            ...options,
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                ...options.headers,
            },
        };

        // Adicionar token se disponível
        const token = this.getStoredToken();
        if (token) {
            config.headers = {
                ...config.headers,
                'Authorization': `Bearer ${token}`,
            };
        }

        try {
            const controller = new AbortController();
            const timeoutId = setTimeout(() => controller.abort(), this.timeout);

            const response = await fetch(url, {
                ...config,
                signal: controller.signal,
            });

            clearTimeout(timeoutId);

            // IMPORTANTE: Não tentar refresh token em endpoints de auth
            const isAuthEndpoint = endpoint.includes('/auth/');

            // Se for 401, tentar refresh token ANTES de tentar ler o body
            if (
                response.status === 401 &&
                !isAuthEndpoint &&
                endpoint !== '/auth/refresh-token'
            ) {
                const refreshResult = await this.refreshToken();
                if (refreshResult.success) {
                    // Tentar novamente com o novo token
                    return this.request<T>(endpoint, options);
                }
                // Se não conseguir renovar, retorna erro padrão
                return {
                    success: false,
                    message: 'Sessão expirada. Faça login novamente.',
                };
            }

            // Verificar se a resposta é JSON
            const contentType = response.headers.get('content-type');
            if (!contentType || !contentType.includes('application/json')) {
                return {
                    success: false,
                    message: 'Resposta inválida do servidor',
                };
            }

            const data: ApiResponse<T> = await response.json();
            return data;

        } catch (error) {
            console.error('API Error:', error);

            if (!(error instanceof Error)) {
                return {
                    success: false,
                    message: 'Erro desconhecido',
                };
            }

            if (error.name === 'AbortError') {
                return {
                    success: false,
                    message: 'Timeout: A requisição demorou muito para responder',
                };
            }

            return {
                success: false,
                message: error.message || 'Erro de conexão com o servidor',
            };
        }
    }

    private async refreshToken(): Promise<ApiResponse<string>> {
        try {
            const response = await fetch(`${this.baseURL}/auth/refresh-token`, {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            const data: ApiResponse<string> = await response.json();

            if (data.success && data.data) {
                // Armazenar o novo token
                localStorage.setItem(STORAGE_KEYS.TOKEN, data.data);
            }

            return data;
        } catch (error) {
            console.error('Refresh token error:', error);
            return {
                success: false,
                message: 'Erro ao renovar token',
            };
        }
    }

    private getStoredToken(): string | null {
        return localStorage.getItem(STORAGE_KEYS.TOKEN);
    }

    // Métodos HTTP com tipagem específica
    async get<T>(endpoint: string): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, { method: 'GET' });
    }

    async post<T, D = unknown>(endpoint: string, data?: D): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, {
            method: 'POST',
            body: data ? JSON.stringify(data) : undefined,
        });
    }

    async put<T, D = unknown>(endpoint: string, data?: D): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, {
            method: 'PUT',
            body: data ? JSON.stringify(data) : undefined,
        });
    }

    async patch<T, D = unknown>(endpoint: string, data?: D): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, {
            method: 'PATCH',
            body: data ? JSON.stringify(data) : undefined,
        });
    }

    async delete<T>(endpoint: string): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, { method: 'DELETE' });
    }
}

export const apiService = new ApiService();