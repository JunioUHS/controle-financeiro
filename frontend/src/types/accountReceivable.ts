import type { Category } from "./category";

export type AccountReceivable = {
    id: number;
    description: string;
    value: number;
    receiptDate: string;
    isReceived: boolean;
    categoryId: number;
    category: Category;
}

export type AccountReceivableFormData = {
    description: string;
    value: number;
    receiptDate: string;
    categoryId: number;
}