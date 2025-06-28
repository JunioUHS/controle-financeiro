import React, { useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { LoginForm } from "./components/LoginForm";
import { Loading } from "../../components/feedback/Loading";

export const Login: React.FC = () => {
  const { isAuthenticated, loading } = useAuth();
  const navigate = useNavigate();

  // Se já estiver logado, redirecionar para dashboard
  useEffect(() => {
    if (!loading && isAuthenticated) {
      navigate("/dashboard", { replace: true });
    }
  }, [isAuthenticated, loading, navigate]);

  // Mostrar loading enquanto verifica autenticação
  if (loading) {
    console.log("Verificando autenticação...");
    return <Loading message="Verificando autenticação..." />;
  }

  // Se já estiver logado, não mostrar a tela de login
  if (isAuthenticated) {
    return null;
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        {/* Cabeçalho */}
        <div>
          <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
            Entre na sua conta
          </h2>
          <p className="mt-2 text-center text-sm text-gray-600">
            Ou{" "}
            <Link
              to="/register"
              className="font-medium text-blue-600 hover:text-blue-500"
            >
              crie uma nova conta
            </Link>
          </p>
        </div>

        {/* Card do formulário */}
        <div className="bg-white py-8 px-6 shadow rounded-lg">
          <LoginForm />
        </div>
      </div>
    </div>
  );
};
