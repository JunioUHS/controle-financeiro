import { apiService } from "./apiService";
import type { ApiResponse } from "@/types/api";
import type { CreditCardPurchase, CreditCardPurchaseFilter, CreditCardPurchaseFormData } from "@/types/creditCardPurchase";

class CreditCardPurchaseService {

    route = "/CreditCardPurchase";

    async getAll(params?: CreditCardPurchaseFilter): Promise<ApiResponse<CreditCardPurchase[]>> {
        let url = `${this.route}/`;
        if (params) {
            const query = new URLSearchParams();
            Object.entries(params).forEach(([key, value]) => {
                if (value !== undefined && value !== null) {
                    query.append(key, String(value));
                }
            });
            if ([...query].length > 0) {
                url += `?${query.toString()}`;
            }
        }
        return apiService.get<CreditCardPurchase[]>(url);
    }

    async getById(id: number): Promise<ApiResponse<CreditCardPurchase>> {
        return apiService.get<CreditCardPurchase>(`${this.route}/${id}`);
    }

    async create(data: CreditCardPurchaseFormData): Promise<ApiResponse<CreditCardPurchase>> {
        return apiService.post<CreditCardPurchase, CreditCardPurchaseFormData>(this.route, data);
    }

    async markInstallmentAsPaid(installmentId: number): Promise<ApiResponse<CreditCardPurchase>> {
        return apiService.patch<CreditCardPurchase>(`${this.route}/Installment/${installmentId}/MarkAsPaid`);
    }

    async delete(id: number): Promise<ApiResponse<void>> {
        return apiService.delete<void>(`${this.route}/${id}`);
    }
}

export const creditCardPurchaseService = new CreditCardPurchaseService();