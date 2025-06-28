import type { Category } from "./category";

export type AccountPayable = {
    id: number;
    description: string;
    value: number;
    dueDate: string;
    isPaid: boolean;
    categoryId: number;
    category: Category;
}

export type AccountPayableFormData = {
    description: string;
    value: number;
    dueDate: string;
    categoryId: number;
}