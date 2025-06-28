import React, { useEffect, useState } from "react";
import { categoryService } from "@/services/categoryService";
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
import { creditCardPurchaseService } from "@/services/creditCardPurchaseService";

const creditCardPurchaseSchema = z.object({
  description: z.string().min(2, "Descrição obrigatória"),
  value: z.coerce.number().min(0.01, "Valor deve ser maior que zero"),
  purchaseDate: z.string().min(1, "Data obrigatória"),
  numberInstallments: z.coerce
    .number()
    .min(1, "Número de parcelas obrigatório"),
  categoryId: z.coerce
    .number()
    .min(1, "Categoria obrigatória")
    .int("Categoria obrigatória"),
});

type CreditCardPurchaseFormData = z.infer<typeof creditCardPurchaseSchema>;

interface Props {
  creditCardId: number;
  open: boolean;
  setOpen: (open: boolean) => void;
  onCreated?: () => void;
}

export const CreditCardPurchaseModal: React.FC<Props> = ({
  creditCardId,
  open,
  setOpen,
  onCreated,
}) => {
  const { handleApiResponse, apiError } = useApiToast();
  const [categories, setCategories] = useState<Category[]>([]);

  const {
    register,
    handleSubmit,
    reset,
    control,
    formState: { errors, isSubmitting },
  } = useForm<CreditCardPurchaseFormData>({
    resolver: zodResolver(creditCardPurchaseSchema),
    defaultValues: {
      description: "",
      value: 0,
      purchaseDate: "",
      numberInstallments: 1,
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

  function handleClose() {
    setOpen(false);
    reset();
  }

  async function onSubmit(data: CreditCardPurchaseFormData) {
    if (isSubmitting) return;

    try {
      const response = await creditCardPurchaseService.create({
        ...data,
        creditCardId,
      });
      const ok = await handleApiResponse(
        response,
        "Compra registrada com sucesso!"
      );

      if (ok) {
        handleClose();
        onCreated?.();
      }
    } catch (err) {
      console.error("Erro ao registrar compra:", err);
      apiError("Erro de conexão", "Não foi possível conectar com o servidor");
    }
  }

  return (
    <ModalBase
      open={open}
      onOpenChange={(v) => {
        if (!v) handleClose();
      }}
      title="Nova Compra"
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
          label="Número de Parcelas"
          placeholder="Número de Parcelas"
          type="number"
          min={1}
          step="1"
          {...register("numberInstallments", { valueAsNumber: true })}
          required
          error={errors.numberInstallments?.message}
        />
        <Input
          label="Data da Compra"
          placeholder="Data da Compra"
          type="date"
          {...register("purchaseDate")}
          required
          error={errors.purchaseDate?.message}
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
