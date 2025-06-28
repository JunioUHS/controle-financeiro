import { categoryService } from "@/services/categoryService";

export async function categoryLoader() {
    const response = await categoryService.getAll();

    if (!response.success) {
        throw new Error("Erro ao carregar contas a pagar");
    }

    return response.data;
}