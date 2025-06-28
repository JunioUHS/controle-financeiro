import { apiService } from "./apiService";
import type { ApiResponse } from "@/types/api";
import type { CreditCard, CreditCardFormData } from "@/types/creditCard";

class CreditCardService {
    async getAll(): Promise<ApiResponse<CreditCard[]>> {
        return apiService.get<CreditCard[]>("/CreditCard");
    }

    async getById(id: number): Promise<ApiResponse<CreditCard>> {
        return apiService.get<CreditCard>(`/CreditCard/${id}`);
    }

    async create(data: CreditCardFormData): Promise<ApiResponse<CreditCard>> {
        return apiService.post<CreditCard, CreditCardFormData>("/CreditCard", data);
    }

    async update(id: number, data: CreditCardFormData): Promise<ApiResponse<CreditCard>> {
        return apiService.put<CreditCard, CreditCardFormData>(`/CreditCard/${id}`, data);
    }
}

export const creditCardService = new CreditCardService();