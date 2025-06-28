import type { User } from "./auth";

export interface AuthContextType {
    user: User | null;
    isAuthenticated: boolean;
    login: (user: User) => void;
    logout: () => void;
    loading: boolean;
}