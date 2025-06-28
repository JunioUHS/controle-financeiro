import type { Category } from "./category";

export type RecurringAccountReceivable = {
    id: number;
    description: string;
    value: number;
    startDate: string;
    endDate?: string | null;
    categoryId: number;
    category: Category;
    isActive: boolean;
}

export type RecurringAccountReceivableFormData = {
    description: string;
    value: number;
    startDate: string;
    endDate?: string | null;
    categoryId: number;
}