import React, { useState, useEffect } from "react";
import { authService } from "../services/authService";
import type { User } from "../types/auth";
import { AuthContext } from "../contexts/AuthContext";

interface AuthProviderProps {
  children: React.ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  // Verificar se usuário está logado ao carregar a aplicação
  useEffect(() => {
    const initAuth = () => {
      try {
        const storedUser = authService.getStoredUser();
        const token = authService.getStoredToken();

        if (storedUser && token) {
          setUser(storedUser);
        }
      } catch (error) {
        console.error("Erro ao inicializar autenticação:", error);
        authService.clearStorage();
      } finally {
        setLoading(false);
      }
    };

    initAuth();
  }, []);

  const login = (userData: User) => {
    setUser(userData);
  };

  const logout = async () => {
    try {
      await authService.logout();
    } catch (error) {
      console.error("Erro no logout:", error);
    } finally {
      setUser(null);
      authService.clearStorage();
    }
  };

  const isAuthenticated = !!user && !!authService.getStoredToken();

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated,
        login,
        logout,
        loading,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
