import React, { useEffect, useState } from "react";
import { categoryService } from "@/services/categoryService";
import { accountReceivableService } from "@/services/accountReceivableService";
import { recurringAccountReceivableService } from "@/services/recurringAccountReceivableService";
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
import type { AccountReceivable } from "@/types/accountReceivable";
import type { ApiResponse } from "@/types";
import type { RecurringAccountReceivable } from "@/types/recurringAccountReceivable";

const accountReceivableSchema = z
  .object({
    description: z.string().min(2, "Descrição obrigatória"),
    value: z.coerce.number().min(0.01, "Valor deve ser maior que zero"),
    receiptDate: z.string().optional(),
    categoryId: z.coerce
      .number()
      .min(1, "Categoria obrigatória")
      .int("Categoria obrigatória"),
    isRecurring: z.boolean().optional(),
    startDate: z.string().optional(),
    endDate: z.string().optional(),
  })
  .refine(
    (data) =>
      !data.isRecurring
        ? !!data.receiptDate && data.receiptDate.length > 0
        : true,
    {
      message: "Data de recebimento é obrigatória.",
      path: ["receiptDate"],
    }
  )
  .refine(
    (data) =>
      !data.isRecurring || (data.startDate && data.startDate.length > 0),
    {
      message: "Data de início é obrigatória para contas recorrentes.",
      path: ["startDate"],
    }
  )
  .refine(
    (data) =>
      !data.endDate ||
      (!!data.startDate && new Date(data.endDate) >= new Date(data.startDate)),
    {
      message: "A data de término não pode ser menor que a data de início.",
      path: ["endDate"],
    }
  );

type AccountReceivableFormData = z.infer<typeof accountReceivableSchema>;

interface Props {
  open: boolean;
  setOpen: (open: boolean) => void;
  onCreated?: () => void;
  editingAccount?: AccountReceivable | null;
  editingRecurringAccount?: RecurringAccountReceivable | null;
}

export const AccountReceivableModal: React.FC<Props> = ({
  open,
  setOpen,
  onCreated,
  editingAccount,
}) => {
  const { handleApiResponse, apiError } = useApiToast();
  const [categories, setCategories] = useState<Category[]>([]);
  const [isRecurring, setIsRecurring] = useState(false);

  const {
    register,
    handleSubmit,
    reset,
    control,
    formState: { errors, isSubmitting },
  } = useForm<AccountReceivableFormData>({
    resolver: zodResolver(accountReceivableSchema),
    defaultValues: {
      description: "",
      value: 0,
      receiptDate: "",
      categoryId: 0,
      isRecurring: false,
      startDate: "",
      endDate: "",
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
      const receiptDate =
        editingAccount.receiptDate.length > 10
          ? editingAccount.receiptDate.slice(0, 10)
          : editingAccount.receiptDate;

      reset({
        description: editingAccount.description,
        value: editingAccount.value,
        receiptDate,
        categoryId: editingAccount.category.id,
        isRecurring: false,
        startDate: editingAccount.receiptDate?.slice(0, 10) || "",
        endDate: "",
      });
    } else {
      reset({
        description: "",
        value: 0,
        receiptDate: "",
        categoryId: 0,
        isRecurring: false,
        startDate: "",
        endDate: "",
      });
    }
  }, [editingAccount, reset]);

  function handleClose() {
    setOpen(false);
    setIsRecurring(false);
    reset();
  }

  async function onSubmit(data: AccountReceivableFormData) {
    if (isSubmitting) return;

    try {
      let response: ApiResponse<AccountReceivable | RecurringAccountReceivable>;
      if (data.isRecurring) {
        response = await recurringAccountReceivableService.create({
          description: data.description,
          value: data.value,
          startDate: data.startDate!,
          endDate: data.endDate || null,
          categoryId: data.categoryId,
        });
      } else if (editingAccount) {
        // Ensure receiptDate is always a string (never undefined)
        const updateData = {
          ...data,
          receiptDate: data.receiptDate!,
        };

        response = await accountReceivableService.update(
          editingAccount.id,
          updateData
        );
      } else {
        const createData = {
          ...data,
          receiptDate: data.receiptDate!,
        };

        response = await accountReceivableService.create(createData);
      }
      const message = editingAccount
        ? "Conta editada com sucesso!"
        : data.isRecurring
        ? "Conta recorrente criada com sucesso!"
        : "Conta criada com sucesso!";
      const ok = await handleApiResponse(response, message);

      if (ok) {
        handleClose();
        onCreated?.();
      }
    } catch (err) {
      console.error("Erro ao criar conta a receber:", err);
      apiError("Erro de conexão", "Não foi possível conectar com o servidor");
    }
  }

  return (
    <ModalBase
      open={open}
      onOpenChange={(v) => {
        if (!v) handleClose();
      }}
      title="Nova Conta a Receber"
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
            form="account-receivable-form"
            disabled={isSubmitting}
          >
            Salvar
          </Button>
        </>
      }
    >
      <form
        id="account-receivable-form"
        className="space-y-4"
        onSubmit={handleSubmit(onSubmit, (errors) => {
          console.log("Form errors:", errors);
        })}
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
        {/* Se NÃO for recorrente, mostra data de recebimento */}
        {!isRecurring && (
          <Input
            label="Data de Recebimento"
            placeholder="Data de Recebimento"
            type="date"
            {...register("receiptDate")}
            required
            error={errors.receiptDate?.message}
          />
        )}
        {/* Se for recorrente, mostra datas de início e término */}
        {isRecurring && (
          <div className="md:flex md:gap-4 md:mb-0">
            <div className="flex-1">
              <Input
                label="Data de Início"
                type="date"
                {...register("startDate")}
                required
                error={errors.startDate?.message}
                className="w-full"
              />
            </div>
            <div className="flex-1 mt-4 md:mt-0 mb-0">
              <Input
                label="Data de término"
                type="date"
                {...register("endDate")}
                error={errors.endDate?.message}
                className="w-full"
              />
            </div>
          </div>
        )}
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
        {/* Checkbox para recorrente */}
        {!editingAccount && (
          <div className="flex items-center gap-2">
            <input
              type="checkbox"
              id="isRecurring"
              {...register("isRecurring")}
              checked={isRecurring}
              onChange={(e) => setIsRecurring(e.target.checked)}
            />
            <label htmlFor="isRecurring" className="text-sm">
              Conta recorrente
            </label>
          </div>
        )}
      </form>
    </ModalBase>
  );
};
