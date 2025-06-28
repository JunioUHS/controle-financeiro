export type CreditCard = {
    id: number;
    name: string;
    limit: number;
    currentBalance: number;
    availableBalance: number;
}

export type CreditCardFormData = {
    name: string;
    limit: number;
}