export type BalanceEvolution = {
    date: string;
    balance: number;
};

export type BalanceSummary = {
    totalReceivable: number;
    totalPayable: number;
    balance: number;
};

export type CreditCardTransaction = {
    cardName: string;
    purchaseDate: string;
    description: string;
    value: number;
    category: string;
};

export type ExpensesByCategory = {
    category: string;
    total: number;
};