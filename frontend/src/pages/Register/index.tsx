import React from "react";
import { useNavigate } from "react-router-dom";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Input } from "@/components/custom-ui/Input";
import { Button } from "@/components/custom-ui/Button";
import { authService } from "@/services/authService";
import { useApiToast } from "@/hooks/useApiToast";

const registerSchema = z
  .object({
    userName: z.string().min(3, "Usuário obrigatório"),
    fullName: z.string().min(3, "Nome completo obrigatório"),
    password: z.string().min(6, "Senha deve ter pelo menos 6 caracteres"),
    confirmPassword: z.string().min(6, "Confirme sua senha"),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "As senhas não conferem",
    path: ["confirmPassword"],
  });

type RegisterFormData = z.infer<typeof registerSchema>;

export const Register: React.FC = () => {
  const navigate = useNavigate();
  const { handleApiResponse, apiError } = useApiToast();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      userName: "",
      fullName: "",
      password: "",
      confirmPassword: "",
    },
  });

  async function onSubmit(data: RegisterFormData) {
    try {
      const response = await authService.register({
        userName: data.userName,
        fullName: data.fullName,
        password: data.password,
        confirmPassword: data.confirmPassword,
      });
      const ok = await handleApiResponse(
        response,
        "Usuário registrado com sucesso!"
      );
      if (ok) {
        navigate("/login");
      }
    } catch {
      apiError("Erro de conexão", "Não foi possível conectar com o servidor");
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div>
          <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
            Crie sua conta
          </h2>
          <p className="mt-2 text-center text-sm text-gray-600">
            Já tem uma conta?{" "}
            <a
              href="/login"
              className="font-medium text-blue-600 hover:text-blue-500"
            >
              Entrar
            </a>
          </p>
        </div>
        <div className="bg-white py-8 px-6 shadow rounded-lg">
          <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
            <Input
              label="Usuário"
              {...register("userName")}
              error={errors.userName?.message}
              required
            />
            <Input
              label="Nome completo"
              {...register("fullName")}
              error={errors.fullName?.message}
              required
            />
            <Input
              label="Senha"
              type="password"
              {...register("password")}
              error={errors.password?.message}
              required
            />
            <Input
              label="Confirmar senha"
              type="password"
              {...register("confirmPassword")}
              error={errors.confirmPassword?.message}
              required
            />
            <Button
              type="submit"
              variant="primary"
              className="w-full"
              loading={isSubmitting}
              disabled={isSubmitting}
            >
              Registrar
            </Button>
          </form>
        </div>
      </div>
    </div>
  );
};
