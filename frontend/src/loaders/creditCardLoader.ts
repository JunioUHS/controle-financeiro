import { creditCardService } from "@/services/creditCardService";

export async function creditCardLoader() {
    const response = await creditCardService.getAll();

    if (!response.success) {
        throw new Error("Erro ao carregar contas a pagar");
    }

    return response.data;
}