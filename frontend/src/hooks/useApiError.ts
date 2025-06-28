import { useCallback } from 'react';
import type { ApiResponse, ApiError } from '../types/api';

interface UseApiErrorReturn<T = unknown> {
    getErrorMessage: (response: ApiResponse<T>) => string;
    getFieldErrors: (response: ApiResponse<T>) => Record<string, string>;
    hasFieldError: (response: ApiResponse<T>, field: string) => boolean;
}

export function useApiError<T = unknown>(): UseApiErrorReturn<T> {
    const getErrorMessage = useCallback((response: ApiResponse<T>): string => {
        if (response.message) {
            return response.message;
        }

        if (response.errors && response.errors.length > 0) {
            return response.errors[0].message;
        }

        return 'Ocorreu um erro inesperado';
    }, []);

    const getFieldErrors = useCallback((response: ApiResponse<T>): Record<string, string> => {
        const fieldErrors: Record<string, string> = {};

        if (response.errors) {
            response.errors.forEach((error: ApiError) => {
                // Converter campo para camelCase para compatibilidade
                const fieldName = error.field.charAt(0).toLowerCase() + error.field.slice(1);
                fieldErrors[fieldName] = error.message;
            });
        }

        return fieldErrors;
    }, []);

    const hasFieldError = useCallback((response: ApiResponse<T>, field: string): boolean => {
        return response.errors?.some((error: ApiError) =>
            error.field.toLowerCase() === field.toLowerCase()
        ) ?? false;
    }, []);

    return {
        getErrorMessage,
        getFieldErrors,
        hasFieldError
    };
}