import { accountReceivableService } from "@/services/accountReceivableService";

export async function accountReceivableLoader() {
    const response = await accountReceivableService.getAll();

    if (!response.success) {
        throw new Error("Erro ao carregar contas a receber");
    }

    return response.data;
}