import { creditCardPurchaseService } from "@/services/creditCardPurchaseService";
import type { CreditCardPurchaseFilter } from "@/types/creditCardPurchase";
import type { LoaderFunctionArgs } from "react-router-dom";

export async function creditCardPurchaseLoader({ params }: LoaderFunctionArgs) {
    if (!params.id) {
        throw new Error("ID do cartão de crédito não fornecido");
    }

    const creditCardId: number = parseInt(params.id);

    const queryParams: CreditCardPurchaseFilter = {
        creditCardId: creditCardId,
    };

    const response = await creditCardPurchaseService.getAll(queryParams);

    if (!response.success) {
        throw new Error("Erro ao carregar contas a pagar");
    }

    return {
        creditCardId,
        initialCreditCardPurchases: response.data,
    };
}