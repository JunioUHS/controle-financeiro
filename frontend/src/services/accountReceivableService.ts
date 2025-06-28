import { apiService } from "./apiService";
import type { ApiResponse } from "@/types/api";
import type { AccountReceivable, AccountReceivableFormData } from "@/types/accountReceivable";

class AccountReceivableService {
    private route = "/AccountReceivable";

    async getAll(): Promise<ApiResponse<AccountReceivable[]>> {
        return apiService.get<AccountReceivable[]>(this.route);
    }

    async create(data: AccountReceivableFormData): Promise<ApiResponse<AccountReceivable>> {
        return apiService.post<AccountReceivable, AccountReceivableFormData>(this.route, data);
    }

    async update(id: number, data: AccountReceivableFormData): Promise<ApiResponse<AccountReceivable>> {
        return apiService.put<AccountReceivable, AccountReceivableFormData>(`${this.route}/${id}`, data);
    }

    async delete(id: number): Promise<ApiResponse<void>> {
        return apiService.delete<void>(`${this.route}/${id}`);
    }

    async markAsReceived(id: number): Promise<ApiResponse<void>> {
        return apiService.patch<void, void>(`${this.route}/${id}/MarkAsReceived`);
    }
}

export const accountReceivableService = new AccountReceivableService();