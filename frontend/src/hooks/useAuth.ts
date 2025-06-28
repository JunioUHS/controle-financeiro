import { useContext } from "react";
import { AuthContext } from "../contexts/AuthContext";
import type { AuthContextType } from "../types/context";

export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth deve ser usado dentro de AuthProvider");
    }
    return context;
};