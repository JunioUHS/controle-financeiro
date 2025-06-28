import React from "react";
import type { CreditCardTransaction } from "@/types/report";
import {
  Table,
  TableHeader,
  TableRow,
  TableHead,
  TableBody,
  TableCell,
} from "@/components/ui/table";
import { formatMoney } from "@/helpers/moneyHelper";

interface Props {
  loading: boolean;
  transactions: CreditCardTransaction[];
}

export const CreditCardTransactionsReport: React.FC<Props> = ({
  loading,
  transactions,
}) => (
  <>
    {loading ? (
      <div>Carregando...</div>
    ) : transactions.length > 0 ? (
      <Table className="bg-white rounded shadow mt-4">
        <TableHeader>
          <TableRow>
            <TableHead>Cartão</TableHead>
            <TableHead>Data</TableHead>
            <TableHead>Descrição</TableHead>
            <TableHead>Categoria</TableHead>
            <TableHead>Valor</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {transactions.map((item, idx) => (
            <TableRow key={idx}>
              <TableCell>{item.cardName}</TableCell>
              <TableCell>
                {new Date(item.purchaseDate).toLocaleDateString("pt-BR")}
              </TableCell>
              <TableCell>{item.description}</TableCell>
              <TableCell>{item.category}</TableCell>
              <TableCell>{formatMoney(item.value)}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    ) : (
      <div>Nenhuma transação encontrada.</div>
    )}
  </>
);
