import { apiService } from "./apiService";
import type {
    BalanceSummary,
    BalanceEvolution,
    ExpensesByCategory,
    CreditCardTransaction,
} from "@/types/report";

export class ReportService {
    async getBalanceSummary(start: string, end: string) {
        return apiService.get<BalanceSummary>(`/Reports/balance-summary?start=${encodeURIComponent(start)}&end=${encodeURIComponent(end)}`);
    }

    async getBalanceEvolution(start: string, end: string) {
        return apiService.get<BalanceEvolution[]>(`/Reports/balance-evolution?start=${encodeURIComponent(start)}&end=${encodeURIComponent(end)}`);
    }

    async getExpensesByCategory(start: string, end: string) {
        return apiService.get<ExpensesByCategory[]>(`/Reports/expenses-by-category?start=${encodeURIComponent(start)}&end=${encodeURIComponent(end)}`);
    }

    async getCreditCardTransactions(creditCardId: number, start: string, end: string) {
        return apiService.get<CreditCardTransaction[]>(`/Reports/credit-card-transactions?creditCardId=${encodeURIComponent(creditCardId)}&start=${encodeURIComponent(start)}&end=${encodeURIComponent(end)}`);
    }
}

export const reportService = new ReportService();