import React, { useState, useRef } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { Input, Button } from "../../../components/custom-ui";
import { authService } from "../../../services/authService";
import { useApiToast } from "../../../hooks/useApiToast";
import { useApiError } from "../../../hooks/useApiError";
import { useAuth } from "../../../hooks/useAuth";
import type { LoginCredentials } from "../../../types/auth";

interface LoginFormProps {
  onSuccess?: () => void;
}

export const LoginForm: React.FC<LoginFormProps> = ({ onSuccess }) => {
  const [formData, setFormData] = useState<LoginCredentials>({
    userName: "",
    password: "",
  });

  const [errors, setErrors] = useState<Partial<LoginCredentials>>({});
  const [loading, setLoading] = useState(false);
  const isSubmitting = useRef(false);

  const { handleApiResponse, apiError } = useApiToast();
  const { getFieldErrors } = useApiError();
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  // Pegar a rota que o usuário tentou acessar antes do login
  const from = location.state?.from?.pathname || "/dashboard";

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));

    if (errors[name as keyof LoginCredentials]) {
      setErrors((prev) => ({
        ...prev,
        [name]: undefined,
      }));
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Partial<LoginCredentials> = {};

    if (!formData.userName.trim()) {
      newErrors.userName = "Nome de usuário é obrigatório";
    }

    if (!formData.password) {
      newErrors.password = "Senha é obrigatória";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm() || isSubmitting.current) return;

    isSubmitting.current = true;
    setLoading(true);

    try {
      const response = await authService.login(formData);

      if (handleApiResponse(response, "Login realizado com sucesso!")) {
        if (response.data) {
          const userData = authService.decodeToken(response.data);
          if (userData) {
            authService.storeUser(userData);
            login(userData); // Atualizar contexto de autenticação

            // Chamar callback se fornecido
            onSuccess?.();

            // Redirecionar para a rota desejada
            navigate(from, { replace: true });
          }
        }
      } else {
        const apiFieldErrors = getFieldErrors(response);
        const formFieldErrors: Partial<LoginCredentials> = {};

        if (apiFieldErrors.userName) {
          formFieldErrors.userName = apiFieldErrors.userName;
        }

        if (apiFieldErrors.password) {
          formFieldErrors.password = apiFieldErrors.password;
        }

        setErrors((prev) => ({ ...prev, ...formFieldErrors }));
      }
    } catch (err) {
      console.error("Erro no login:", err);
      apiError("Erro de conexão", "Não foi possível conectar com o servidor");
    } finally {
      setLoading(false);
      isSubmitting.current = false;
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      <Input
        label="Nome de usuário"
        type="text"
        name="userName"
        value={formData.userName}
        onChange={handleChange}
        error={errors.userName}
        placeholder="Digite seu nome de usuário"
        required
      />

      <Input
        label="Senha"
        type="password"
        name="password"
        value={formData.password}
        onChange={handleChange}
        error={errors.password}
        placeholder="Digite sua senha"
        required
      />

      {/* <div className="flex items-center justify-between">
        <RememberMeCheckbox checked={rememberMe} onChange={setRememberMe} />
        <ForgotPasswordLink onClick={handleForgotPassword} />
      </div> */}

      <Button
        className="mt-2"
        type="submit"
        loading={loading}
        disabled={loading}
      >
        Entrar
      </Button>
    </form>
  );
};
