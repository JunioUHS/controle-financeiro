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
import type { AccountReceivable } from "@/types/accountReceivable";
import { formatDate } from "@/helpers/dateHelper";
import { Loading } from "@/components";
import { formatMoney } from "@/helpers/moneyHelper";

interface AccountReceivableListProps {
  accounts: AccountReceivable[];
  loading?: boolean;
  onEdit: (account: AccountReceivable) => void;
  onDelete: (id: number) => void;
  onMarkAsReceived: (id: number) => void;
}

export const AccountReceivableList: React.FC<AccountReceivableListProps> = ({
  accounts,
  loading = false,
  onEdit,
  onDelete,
  onMarkAsReceived,
}) => {
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
              Data de Recebimento
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
                Nenhuma conta cadastrada.
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
                  {formatDate(account.receiptDate)}
                </TableCell>
                <TableCell className="px-4 py-3">
                  <span
                    className={`px-2 py-1 rounded text-xs font-semibold
                    ${
                      account.isReceived
                        ? "bg-green-100 text-green-700"
                        : "bg-yellow-100 text-yellow-700"
                    }`}
                  >
                    {account.isReceived ? "Recebido" : "Pendente"}
                  </span>
                </TableCell>
                <TableCell className="px-4 py-3">
                  {account.category.name}
                </TableCell>
                <TableCell className="px-4 py-3 flex gap-2 justify-center">
                  <Button
                    type="button"
                    variant="secondary"
                    className="!w-auto px-3 py-1"
                    onClick={() => onEdit(account)}
                  >
                    Editar
                  </Button>
                  <Button
                    type="button"
                    variant="danger"
                    className="!w-auto px-3 py-1"
                    onClick={() => onDelete(account.id)}
                  >
                    Excluir
                  </Button>
                  {!account.isReceived && (
                    <Button
                      type="button"
                      variant="success"
                      className="!w-auto px-3 py-1"
                      onClick={() => onMarkAsReceived(account.id)}
                    >
                      Marcar como Recebido
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
