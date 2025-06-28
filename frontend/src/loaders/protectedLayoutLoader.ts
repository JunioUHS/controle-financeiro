import { redirect } from "react-router-dom";
import { authService } from "../services/authService";

export async function protectedLayoutLoader() {
    const token = authService.getStoredToken();
    const user = authService.getStoredUser();

    if (!token || !user) {
        throw redirect("/login");
    }

    const res = await authService.getCurrentUser();

    // Se falhar a requisição, redireciona para login
    if (!res.success || !res.data) {
        throw redirect("/login");
    }

    // Retorna o usuário atualizado da API
    return { user: res.data };
}