import { accountPayableService } from "@/services/accountPayableService";

export async function accountPayableLoader() {
    const response = await accountPayableService.getAll();

    if (!response.success) {
        throw new Error("Erro ao carregar contas a pagar");
    }

    return response.data;
}