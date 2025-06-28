import type { RecurringAccountReceivable, RecurringAccountReceivableFormData } from "@/types/recurringAccountReceivable";
import { apiService } from "./apiService";
import type { ApiResponse } from "@/types/api";

class RecurringAccountReceivableService {
    private route = "/RecurringAccountReceivable";

    async create(data: RecurringAccountReceivableFormData): Promise<ApiResponse<RecurringAccountReceivable>> {
        return apiService.post<RecurringAccountReceivable, RecurringAccountReceivableFormData>(this.route, data);
    }

    async getAll(): Promise<ApiResponse<RecurringAccountReceivable[]>> {
        return apiService.get<RecurringAccountReceivable[]>(this.route);
    }

    async getById(id: number): Promise<ApiResponse<RecurringAccountReceivable>> {
        return apiService.get<RecurringAccountReceivable>(`${this.route}/${id}`);
    }

    async update(id: number, data: RecurringAccountReceivableFormData): Promise<ApiResponse<RecurringAccountReceivable>> {
        return apiService.put<RecurringAccountReceivable, RecurringAccountReceivableFormData>(`${this.route}/${id}`, data);
    }

    async delete(id: number): Promise<ApiResponse<void>> {
        return apiService.delete<void>(`${this.route}/${id}`);
    }

    async inactivate(id: number): Promise<ApiResponse<void>> {
        return apiService.patch<void>(`${this.route}/${id}/Inactivate`);
    }
}

export const recurringAccountReceivableService = new RecurringAccountReceivableService();