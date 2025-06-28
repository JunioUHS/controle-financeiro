import React, { useEffect } from "react";
import { categoryService } from "@/services/categoryService";
import { Button } from "@/components/custom-ui/Button";
import { Input } from "@/components/custom-ui/Input";
import { DialogClose } from "@/components/ui/dialog";
import { useApiToast } from "@/hooks/useApiToast";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import type { Category } from "@/types/category";
import { ModalBase } from "@/components/custom-ui/ModalBase";
import type { ApiResponse } from "@/types";

const categorySchema = z.object({
  name: z.string().min(2, "Nome obrigatório"),
});

type CategoryFormData = z.infer<typeof categorySchema>;

interface Props {
  open: boolean;
  setOpen: (open: boolean) => void;
  onCreated?: () => void;
  editingCategory?: Category | null;
}

export const CategoryModal: React.FC<Props> = ({
  open,
  setOpen,
  onCreated,
  editingCategory,
}) => {
  const { handleApiResponse, apiError } = useApiToast();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<CategoryFormData>({
    resolver: zodResolver(categorySchema),
    defaultValues: {
      name: "",
    },
  });

  useEffect(() => {
    if (editingCategory) {
      reset({
        name: editingCategory.name,
      });
    } else {
      reset({
        name: "",
      });
    }
  }, [editingCategory, reset]);

  function handleClose() {
    setOpen(false);
    reset();
  }

  async function onSubmit(data: CategoryFormData) {
    if (isSubmitting) return;

    try {
      let response: ApiResponse<Category>;
      if (editingCategory) {
        response = await categoryService.update(editingCategory.id, data);
      } else {
        response = await categoryService.create(data);
      }
      const message = editingCategory
        ? "Categoria editada com sucesso!"
        : "Categoria criada com sucesso!";
      const ok = await handleApiResponse(response, message);

      if (ok) {
        handleClose();
        onCreated?.();
      }
    } catch (err) {
      console.error("Erro ao criar categoria:", err);
      apiError("Erro de conexão", "Não foi possível conectar com o servidor");
    }
  }

  return (
    <ModalBase
      open={open}
      onOpenChange={(v) => {
        if (!v) handleClose();
      }}
      title="Nova Categoria"
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
          label="Nome da Categoria"
          placeholder="Nome da Categoria"
          {...register("name")}
          error={errors.name?.message}
        />
      </form>
    </ModalBase>
  );
};
