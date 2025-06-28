import React, { useState } from "react";
import { Button } from "@/components/custom-ui/Button";
import { TitlePage } from "@/components/layout/TitlePage";
import { useLoaderData } from "react-router-dom";
import type { Category } from "@/types/category";
import { categoryService } from "@/services/categoryService";
import { CategoryList } from "./components/CategoryList";
import { CategoryModal } from "./components/CategoryModal";

export const CategoryPage: React.FC = () => {
  const initialCategories = useLoaderData() as Category[];
  const [categories, setCategories] = useState(initialCategories);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);

  async function reloadCategories() {
    const res = await categoryService.getAll();
    if (res.success) setCategories(res.data ?? []);
  }

  return (
    <>
      <TitlePage title="Categorias">
        <Button
          type="button"
          variant="primary"
          className="!w-auto px-4 py-2"
          onClick={() => setModalOpen(true)}
        >
          Novo
        </Button>
      </TitlePage>

      <CategoryModal
        open={modalOpen}
        setOpen={(open) => {
          setModalOpen(open);
          if (!open) setEditingCategory(null);
        }}
        onCreated={reloadCategories}
        editingCategory={editingCategory}
      />

      <div className="overflow-x-auto bg-white rounded-lg shadow">
        <CategoryList
          categories={categories}
          onEdit={(category) => {
            setEditingCategory(category);
            setModalOpen(true);
          }}
        />
      </div>
    </>
  );
};
