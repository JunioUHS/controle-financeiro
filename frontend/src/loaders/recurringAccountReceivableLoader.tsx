import { recurringAccountReceivableService } from "@/services/recurringAccountReceivableService";

export async function recurringAccountReceivableLoader() {
  const response = await recurringAccountReceivableService.getAll();

  if (!response.success) {
    throw new Error("Erro ao carregar contas a receber");
  }

  return response.data;
}
