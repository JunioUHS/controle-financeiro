import React from "react";
import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableHead,
  TableCell,
} from "@/components/ui/table";
import { Button } from "@/components/custom-ui/Button";
import { formatDate } from "@/helpers/dateHelper";
import { Loading } from "@/components";
import type { CreditCardPurchase } from "@/types/creditCardPurchase";
import { formatMoney } from "@/helpers/moneyHelper";

interface CreditCardPurchaseListProps {
  creditCardPurchases: CreditCardPurchase[];
  loading?: boolean;
  onDelete: (id: number) => void;
  onShowInstallments: (purchase: CreditCardPurchase) => void;
}

export const CreditCardPurchaseList: React.FC<CreditCardPurchaseListProps> = ({
  creditCardPurchases,
  loading = false,
  onDelete,
  onShowInstallments,
}) => {
  if (loading) {
    return (
      <div className="bg-white rounded-lg shadow min-h-[200px] flex items-center justify-center">
        <Loading message="Carregando compras..." />
      </div>
    );
  }

  return (
    <div className="overflow-x-auto bg-white rounded-lg shadow">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Descrição
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Valor
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Data da Compra
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Número de Parcelas
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Categoria
            </TableHead>
            <TableHead className="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">
              Ações
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {creditCardPurchases.length === 0 ? (
            <TableRow>
              <TableCell
                colSpan={6}
                className="px-4 py-6 text-center text-gray-400"
              >
                Nenhuma compra cadastrada.
              </TableCell>
            </TableRow>
          ) : (
            creditCardPurchases.map((creditCardPurchase) => (
              <TableRow
                key={creditCardPurchase.id}
                className="hover:bg-gray-50 transition"
              >
                <TableCell className="px-4 py-3">
                  {creditCardPurchase.description}
                </TableCell>
                <TableCell className="px-4 py-3">
                  {formatMoney(creditCardPurchase.value)}
                </TableCell>
                <TableCell className="px-4 py-3">
                  {formatDate(creditCardPurchase.purchaseDate)}
                </TableCell>
                {/* <TableCell className="px-4 py-3">
                  <span
                    className={`px-2 py-1 rounded text-xs font-semibold
                    ${
                      creditCardPurchase.isPaid
                        ? "bg-green-100 text-green-700"
                        : "bg-yellow-100 text-yellow-700"
                    }`}
                  >
                    {account.isPaid ? "Paga" : "Pendente"}
                  </span>
                </TableCell> */}
                <TableCell className="px-4 py-3">
                  {creditCardPurchase.numberInstallments}
                </TableCell>
                <TableCell className="px-4 py-3">
                  {creditCardPurchase.category.name}
                </TableCell>
                <TableCell className="px-4 py-3 flex gap-2 justify-center">
                  <Button
                    type="button"
                    variant="secondary"
                    className="!w-auto px-3 py-1"
                    onClick={() => onShowInstallments(creditCardPurchase)}
                  >
                    Parcelas
                  </Button>
                  <Button
                    type="button"
                    variant="danger"
                    className="!w-auto px-3 py-1"
                    onClick={() => onDelete(creditCardPurchase.id)}
                  >
                    Excluir
                  </Button>
                </TableCell>
              </TableRow>
            ))
          )}
        </TableBody>
      </Table>
    </div>
  );
};
