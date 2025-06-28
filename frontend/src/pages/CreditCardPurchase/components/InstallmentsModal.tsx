import React from "react";
import { ModalBase } from "@/components/custom-ui/ModalBase";
import type { CreditCardPurchase } from "@/types/creditCardPurchase";
import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableHead,
  TableCell,
} from "@/components/ui/table";
import { DialogClose } from "@/components/ui/dialog";
import { Button } from "@/components";
import { formatDate } from "@/helpers/dateHelper";
import { formatMoney } from "@/helpers/moneyHelper";

interface InstallmentsModalProps {
  open: boolean;
  setOpen: (open: boolean) => void;
  purchase: CreditCardPurchase | null;
  onMarkInstallmentAsPaid?: (installmentId: number) => void; // NOVO
}

export const InstallmentsModal: React.FC<InstallmentsModalProps> = ({
  open,
  setOpen,
  purchase,
  onMarkInstallmentAsPaid, // NOVO
}) => {
  if (!purchase) return null;

  function handleClose() {
    setOpen(false);
  }

  return (
    <ModalBase
      open={open}
      onOpenChange={setOpen}
      title={`Parcelas da compra: ${purchase.description}`}
      maxWidth="max-w-lg"
      footer={
        <>
          <DialogClose asChild>
            <Button type="button" variant="secondary" onClick={handleClose}>
              Cancelar
            </Button>
          </DialogClose>
        </>
      }
    >
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="text-left py-2">Nº</TableHead>
            <TableHead className="text-left py-2">Valor</TableHead>
            <TableHead className="text-left py-2">Vencimento</TableHead>
            <TableHead className="text-left py-2">Status</TableHead>
            <TableHead className="py-2 text-center">Ações</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {(purchase.installments ?? []).map((inst) => (
            <TableRow key={inst.id}>
              <TableCell className="py-2">{inst.numberInstallment}</TableCell>
              <TableCell className="py-2">{formatMoney(inst.value)}</TableCell>
              <TableCell className="py-2">{formatDate(inst.dueDate)}</TableCell>
              <TableCell className="py-2">
                <span
                  className={`px-2 py-1 rounded text-xs font-semibold
                    ${
                      inst.isPaid
                        ? "bg-green-100 text-green-700"
                        : "bg-yellow-100 text-yellow-700"
                    }`}
                >
                  {inst.isPaid ? "Paga" : "Pendente"}
                </span>
              </TableCell>
              <TableCell className="py-2 text-center">
                {!inst.isPaid && (
                  <Button
                    type="button"
                    variant="success"
                    className="!w-auto px-3 py-1"
                    onClick={() =>
                      onMarkInstallmentAsPaid &&
                      onMarkInstallmentAsPaid(inst.id)
                    }
                  >
                    Marcar como Paga
                  </Button>
                )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </ModalBase>
  );
};
