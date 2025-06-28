import React, { useEffect, useState } from "react";
import { categoryService } from "@/services/categoryService";
import { accountPayableService } from "@/services/accountPayableService";
import { Button } from "@/components/custom-ui/Button";
import { Input } from "@/components/custom-ui/Input";
import { DialogClose } from "@/components/ui/dialog";
import { useApiToast } from "@/hooks/useApiToast";
import { z } from "zod";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import type { Category } from "@/types/category";
import { Combobox } from "@/components/custom-ui/Combobox";
import { ModalBase } from "@/components/custom-ui/ModalBase";
import type { AccountPayable } from "@/types/accountPayable";
import type { ApiResponse } from "@/types";

const accountPayableSchema = z.object({
  description: z.string().min(2, "Descrição obrigatória"),
  value: z.coerce.number().min(0.01, "Valor deve ser maior que zero"),
  dueDate: z.string().min(1, "Data obrigatória"),
  categoryId: z.coerce
    .number()
    .min(1, "Categoria obrigatória")
    .int("Categoria obrigatória"),
});

type AccountPayableFormData = z.infer<typeof accountPayableSchema>;

interface Props {
  open: boolean;
  setOpen: (open: boolean) => void;
  onCreated?: () => void;
  editingAccount?: AccountPayable | null;
}

export const AccountPayableModal: React.FC<Props> = ({
  open,
  setOpen,
  onCreated,
  editingAccount,
}) => {
  const { handleApiResponse, apiError } = useApiToast();
  const [categories, setCategories] = useState<Category[]>([]);

  const {
    register,
    handleSubmit,
    reset,
    control,
    formState: { errors, isSubmitting },
  } = useForm<AccountPayableFormData>({
    resolver: zodResolver(accountPayableSchema),
    defaultValues: {
      description: "",
      value: 0,
      dueDate: "",
      categoryId: 0,
    },
  });

  useEffect(() => {
    if (open) {
      categoryService.getAll().then((res) => {
        if (res.success) setCategories(res.data ?? []);
      });
    }
  }, [open]);

  useEffect(() => {
    if (editingAccount) {
      // Ajuste para garantir o formato YYYY-MM-DD
      const dueDate =
        editingAccount.dueDate.length > 10
          ? editingAccount.dueDate.slice(0, 10)
          : editingAccount.dueDate;

      reset({
        description: editingAccount.description,
        value: editingAccount.value,
        dueDate,
        categoryId: editingAccount.category.id,
      });
    } else {
      reset({
        description: "",
        value: 0,
        dueDate: "",
        categoryId: 0,
      });
    }
  }, [editingAccount, reset]);

  function handleClose() {
    setOpen(false);
    reset();
  }

  async function onSubmit(data: AccountPayableFormData) {
    if (isSubmitting) return;

    try {
      let response: ApiResponse<AccountPayable>;
      if (editingAccount) {
        response = await accountPayableService.update(editingAccount.id, data);
      } else {
        response = await accountPayableService.create(data);
      }
      const message = editingAccount
        ? "Conta editada com sucesso!"
        : "Conta criada com sucesso!";
      const ok = await handleApiResponse(response, message);

      if (ok) {
        handleClose();
        onCreated?.();
      }
    } catch (err) {
      console.error("Erro ao criar conta a pagar:", err);
      apiError("Erro de conexão", "Não foi possível conectar com o servidor");
    }
  }

  return (
    <ModalBase
      open={open}
      onOpenChange={(v) => {
        if (!v) handleClose();
      }}
      title="Nova Conta a Pagar"
      maxWidth="max-w-7xl"
      footer={
        <>
          <DialogClose asChild>
            <Button type="button" variant="secondary" onClick={handleClose}>
              Cancelar
            </Button>
          </DialogClose>
          <Button
            type="submit"
            variant="primary"
            form="account-payable-form"
            disabled={isSubmitting}
          >
            Salvar
          </Button>
        </>
      }
    >
      <form
        id="account-payable-form"
        className="space-y-4"
        onSubmit={handleSubmit(onSubmit)}
      >
        <Input
          label="Descrição"
          placeholder="Descrição"
          {...register("description")}
          error={errors.description?.message}
        />
        <Input
          label="Valor"
          placeholder="Valor"
          type="number"
          min={0}
          step="0.01"
          {...register("value", { valueAsNumber: true })}
          required
          error={errors.value?.message}
        />
        <Input
          label="Data de Vencimento"
          placeholder="Data de Vencimento"
          type="date"
          {...register("dueDate")}
          required
          error={errors.dueDate?.message}
        />
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Categoria
          </label>
          <Controller
            name="categoryId"
            control={control}
            rules={{ required: true }}
            render={({ field }) => (
              <Combobox
                options={categories.map((cat) => ({
                  value: cat.id,
                  label: cat.name,
                }))}
                value={field.value}
                onChange={field.onChange}
                placeholder="Selecione uma categoria"
                error={errors.categoryId?.message}
              />
            )}
          />
        </div>
      </form>
    </ModalBase>
  );
};
