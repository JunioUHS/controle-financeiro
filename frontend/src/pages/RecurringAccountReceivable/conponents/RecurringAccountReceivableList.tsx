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
import { formatMoney } from "@/helpers/moneyHelper";
import type { RecurringAccountReceivable } from "@/types/recurringAccountReceivable";

interface RecurringAccountReceivableListProps {
  accounts: RecurringAccountReceivable[];
  loading?: boolean;
  onDelete: (id: number) => void;
  onInactivate: (id: number) => void;
}

export const RecurringAccountReceivableList: React.FC<
  RecurringAccountReceivableListProps
> = ({ accounts, loading = false, onDelete, onInactivate }) => {
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
              Descrição
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Valor
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Data de Inicio
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Data de Término
            </TableHead>
            <TableHead className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">
              Status
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
          {accounts.length === 0 ? (
            <TableRow>
              <TableCell
                colSpan={6}
                className="px-4 py-6 text-center text-gray-400"
              >
                Nenhuma conta recorrente cadastrada.
              </TableCell>
            </TableRow>
          ) : (
            accounts.map((account) => (
              <TableRow
                key={account.id}
                className="hover:bg-gray-50 transition"
              >
                <TableCell className="px-4 py-3">
                  {account.description}
                </TableCell>
                <TableCell className="px-4 py-3">
                  {formatMoney(account.value)}
                </TableCell>
                <TableCell className="px-4 py-3">
                  {formatDate(account.startDate)}
                </TableCell>
                <TableCell className="px-4 py-3">
                  {formatDate(account.startDate)}
                </TableCell>
                <TableCell className="px-4 py-3">
                  <span
                    className={`px-2 py-1 rounded text-xs font-semibold
                    ${
                      account.isActive
                        ? "bg-green-100 text-green-700"
                        : "bg-red-100 text-red-700"
                    }`}
                  >
                    {account.isActive ? "Ativo" : "Inativo"}
                  </span>
                </TableCell>
                <TableCell className="px-4 py-3">
                  {account.category.name}
                </TableCell>
                <TableCell className="px-4 py-3 flex gap-2 justify-center">
                  <Button
                    type="button"
                    variant="danger"
                    className="!w-auto px-3 py-1"
                    onClick={() => onDelete(account.id)}
                  >
                    Excluir
                  </Button>
                  {account.isActive && (
                    <Button
                      type="button"
                      variant="warning"
                      className="!w-auto px-3 py-1"
                      onClick={() => onInactivate(account.id)}
                    >
                      Inativar
                    </Button>
                  )}
                </TableCell>
              </TableRow>
            ))
          )}
        </TableBody>
      </Table>
    </div>
  );
};
