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
import type { Category } from "@/types/category";

interface CategoryListProps {
  categories: Category[];
  loading?: boolean;
  onEdit: (category: Category) => void;
}

export const CategoryList: React.FC<CategoryListProps> = ({
  categories,
  loading = false,
  onEdit,
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
            <TableHead className="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">
              Ações
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {categories.length === 0 ? (
            <TableRow>
              <TableCell
                colSpan={6}
                className="px-4 py-6 text-center text-gray-400"
              >
                Nenhuma conta cadastrada.
              </TableCell>
            </TableRow>
          ) : (
            categories.map((category) => (
              <TableRow
                key={category.id}
                className="hover:bg-gray-50 transition"
              >
                <TableCell className="px-4 py-3">{category.name}</TableCell>

                <TableCell className="px-4 py-3 flex gap-2 justify-center">
                  <Button
                    type="button"
                    variant="secondary"
                    className="!w-auto px-3 py-1"
                    onClick={() => onEdit(category)}
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
