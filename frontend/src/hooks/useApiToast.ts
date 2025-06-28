import { toast } from 'sonner';
import { useApiError } from './useApiError';
import type { ApiResponse } from '../types/api';

export function useApiToast() {
    const { getErrorMessage, getFieldErrors } = useApiError();

    const handleApiResponse = <T>(
        response: ApiResponse<T>,
        successMessage?: string,
        errorTitle = "Erro"
    ) => {
        if (response.success) {
            if (successMessage) {
                toast.success(successMessage);
            }
            return true;
        } else {
            const fieldErrors = getFieldErrors(response);

            if (Object.keys(fieldErrors).length > 0) {
                const errorMessages = Object.values(fieldErrors);
                toast.error("Erro de validação", {
                    description: errorMessages.join(', '),
                });
            } else {
                toast.error(errorTitle, {
                    description: getErrorMessage(response),
                });
            }
            return false;
        }
    };

    const apiSuccess = (message: string, description?: string) => {
        toast.success(message, { description });
    };

    const apiError = (message: string, description?: string) => {
        toast.error(message, { description });
    };

    const apiInfo = (message: string, description?: string) => {
        toast.info(message, { description });
    };

    const apiWarning = (message: string, description?: string) => {
        toast.warning(message, { description });
    };

    return {
        handleApiResponse,
        apiSuccess,
        apiError,
        apiInfo,
        apiWarning,
    };
}