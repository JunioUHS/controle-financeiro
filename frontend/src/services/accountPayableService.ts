import type { AccountPayable, AccountPayableFormData } from "@/types/accountPayable";
import { apiService } from "./apiService";
import type { ApiResponse } from "@/types/api";

export class AccountPayableService {

    route = "/AccountPayable";

    async getAll(): Promise<ApiResponse<AccountPayable[]>> {
        return apiService.get<AccountPayable[]>(this.route);
    }

    async getById(id: number): Promise<ApiResponse<AccountPayable>> {
        return apiService.get<AccountPayable>(`${this.route}/${id}`);
    }

    async create(data: AccountPayableFormData): Promise<ApiResponse<AccountPayable>> {
        return apiService.post<AccountPayable>(this.route, data);
    }

    async update(id: number, data: AccountPayableFormData): Promise<ApiResponse<AccountPayable>> {
        return apiService.put<AccountPayable>(`${this.route}/${id}`, data);
    }

    async delete(id: number): Promise<ApiResponse<void>> {
        return apiService.delete<void>(`${this.route}/${id}`);
    }

    async markAsPaid(id: number): Promise<ApiResponse<AccountPayable>> {
        return apiService.patch<AccountPayable>(`${this.route}/${id}/MarkAsPaid`);
    }
}

export const accountPayableService = new AccountPayableService();