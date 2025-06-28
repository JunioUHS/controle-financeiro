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
import { Loading } from "@/components";
import type { CreditCard } from "@/types/creditCard";
import { useNavigate } from "react-router-dom";
import { formatMoney } from "@/helpers/moneyHelper";

interface CreditCardListProps {
  creditCards: CreditCard[];
  loading?: boolean;
  onEdit: (creditCard: CreditCard) => void;
}

export const CreditCardList: React.FC<CreditCardListProps> = ({
  creditCards,
  loading = false,
  onEdit,
}) => {
  const navigate = useNavigate();

  if (loading) {
    return (
      <div className="bg-white rounded-lg shadow min-h-[200px] flex items-center justify-center">
        <Loading message="Carregando contas..." />
      </div>
    );
  }

  return (
    <div className="overflow-x-auto bg-white rounded-lg shadow">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Nome
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Limite
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Total Gasto
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Saldo Disponível
            </TableHead>
            <TableHead className="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">
              Ações
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {creditCards.length === 0 ? (
            <TableRow>
              <TableCell
                colSpan={6}
                className="px-4 py-6 text-center text-gray-400"
              >
                Nenhuma conta cadastrada.
              </TableCell>
            </TableRow>
          ) : (
            creditCards.map((creditCard) => (
              <TableRow
                key={creditCard.id}
                className="hover:bg-gray-50 transition"
              >
                <TableCell className="px-4 py-3">{creditCard.name}</TableCell>

                <TableCell className="px-4 py-3">
                  {formatMoney(creditCard.limit)}
                </TableCell>

                <TableCell className="px-4 py-3">
                  {formatMoney(creditCard.currentBalance)}
                </TableCell>

                <TableCell className="px-4 py-3">
                  {formatMoney(creditCard.availableBalance)}
                </TableCell>

                <TableCell className="px-4 py-3 flex gap-2 justify-center">
                  <Button
                    type="button"
                    variant="primary"
                    className="!w-auto px-3 py-1"
                    onClick={() =>
                      navigate(`/credit-cards/${creditCard.id}/purchases`)
                    }
                  >
                    Compras
                  </Button>
                  <Button
                    type="button"
                    variant="secondary"
                    className="!w-auto px-3 py-1"
                    onClick={() => onEdit(creditCard)}
                  >
                    Editar
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
