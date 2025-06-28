import React, { useEffect } from "react";
import { Button } from "@/components/custom-ui/Button";
import { Input } from "@/components/custom-ui/Input";
import { DialogClose } from "@/components/ui/dialog";
import { useApiToast } from "@/hooks/useApiToast";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import type { CreditCard } from "@/types/creditCard";
import { ModalBase } from "@/components/custom-ui/ModalBase";
import type { ApiResponse } from "@/types";
import { creditCardService } from "@/services/creditCardService";

const creditCardSchema = z.object({
  name: z.string().min(2, "Nome obrigatório"),
  limit: z.number().min(0.1, "Limite deve ser maior que zero"),
});

type CreditCardFormData = z.infer<typeof creditCardSchema>;

interface Props {
  open: boolean;
  setOpen: (open: boolean) => void;
  onCreated?: () => void;
  editingCreditCard?: CreditCard | null;
}

export const CreditCardModal: React.FC<Props> = ({
  open,
  setOpen,
  onCreated,
  editingCreditCard,
}) => {
  const { handleApiResponse, apiError } = useApiToast();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<CreditCardFormData>({
    resolver: zodResolver(creditCardSchema),
    defaultValues: {
      name: "",
      limit: 0,
    },
  });

  useEffect(() => {
    if (editingCreditCard) {
      reset({
        name: editingCreditCard.name,
        limit: editingCreditCard.limit,
      });
    } else {
      reset({
        name: "",
        limit: 0,
      });
    }
  }, [editingCreditCard, reset]);

  function handleClose() {
    setOpen(false);
    reset();
  }

  async function onSubmit(data: CreditCardFormData) {
    if (isSubmitting) return;

    try {
      let response: ApiResponse<CreditCard>;
      if (editingCreditCard) {
        response = await creditCardService.update(editingCreditCard.id, data);
      } else {
        response = await creditCardService.create(data);
      }
      const message = editingCreditCard
        ? "Cartão de crédito editado com sucesso!"
        : "Cartão de crédito criado com sucesso!";
      const ok = await handleApiResponse(response, message);

      if (ok) {
        handleClose();
        onCreated?.();
      }
    } catch (err) {
      console.error("Erro ao criar cartão de crédito:", err);
      apiError("Erro de conexão", "Não foi possível conectar com o servidor");
    }
  }

  return (
    <ModalBase
      open={open}
      onOpenChange={(v) => {
        if (!v) handleClose();
      }}
      title="Novo Cartão de Crédito"
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
          label="Nome"
          placeholder="Nome"
          {...register("name")}
          error={errors.name?.message}
        />
        <Input
          label="Limite"
          placeholder="Limite"
          type="number"
          min={0}
          step="0.01"
          {...register("limit", { valueAsNumber: true })}
          required
          error={errors.limit?.message}
        />
      </form>
    </ModalBase>
  );
};
