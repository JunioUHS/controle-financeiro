import type { ExpensesByCategory } from "@/types/report";
import React from "react";
import {
  Table,
  TableHeader,
  TableRow,
  TableHead,
  TableBody,
  TableCell,
} from "@/components/ui/table";
import { formatMoney } from "@/helpers/moneyHelper";

interface ExpensesByCategoryReportProps {
  loading: boolean;
  expensesByCategory: ExpensesByCategory[];
}

export const ExpensesByCategoryReport: React.FC<
  ExpensesByCategoryReportProps
> = ({ loading, expensesByCategory }) => (
  <>
    {loading ? (
      <div>Carregando...</div>
    ) : expensesByCategory.length > 0 ? (
      <Table className="bg-white rounded shadow mt-4">
        <TableHeader>
          <TableRow>
            <TableHead>Categoria</TableHead>
            <TableHead>Total</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {expensesByCategory.map((item: ExpensesByCategory) => (
            <TableRow key={item.category}>
              <TableCell>{item.category}</TableCell>
              <TableCell>{formatMoney(item.total)}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    ) : (
      <div>Nenhum dado encontrado.</div>
    )}
  </>
);
