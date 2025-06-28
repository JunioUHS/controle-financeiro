import type { Category } from "./category";
import type { CreditCard } from "./creditCard";

export type CreditCardPurchase = {
    id: number;
    description: string;
    value: number;
    purchaseDate: string;
    numberInstallments: number;
    categoryId: number;
    category: Category;
    creditCardId: number;
    creditCard: CreditCard;
    installments: PurchaseInstallment[];
}

export type CreditCardPurchaseFormData = {
    description: string;
    value: number;
    purchaseDate: string;
    numberInstallments: number;
    categoryId: number;
    creditCardId: number;
}

export type CreditCardPurchaseFilter = {
    creditCardId?: number;
    categoryId?: number;
}

export type PurchaseInstallment = {
    id: number;
    numberInstallment: number;
    dueDate: string;
    value: number;
    isPaid: boolean;
}