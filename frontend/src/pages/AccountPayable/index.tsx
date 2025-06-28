import React, { useState } from "react";
import { Button } from "@/components/custom-ui/Button";
import { TitlePage } from "@/components/layout/TitlePage";
import { AccountPayableModal } from "./components/AccountPayableModal";
import { AccountPayableList } from "./components/AccountPayableList";
import { useLoaderData } from "react-router-dom";
import type { AccountPayable } from "@/types/accountPayable";
import { accountPayableService } from "@/services/accountPayableService";
import { useApiToast } from "@/hooks/useApiToast";

export const AccountPayablePage: React.FC = () => {
  const initialAccounts = useLoaderData() as AccountPayable[];
  const [accounts, setAccounts] = useState(initialAccounts);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingAccount, setEditingAccount] = useState<AccountPayable | null>(
    null
  );
  const { handleApiResponse } = useApiToast();

  async function reloadAccounts() {
    const res = await accountPayableService.getAll();
    if (res.success) setAccounts(res.data ?? []);
  }

  async function handleMarkAsPaid(id: number) {
    const res = await accountPayableService.markAsPaid(id);
    const handleApi = handleApiResponse(res, "Conta marcada como paga");
    if (handleApi) {
      reloadAccounts();
    }
  }

  async function handleDelete(id: number) {
    const res = await accountPayableService.delete(id);
    const ok = await handleApiResponse(res, "Conta excluída com sucesso!");
    if (ok) {
      reloadAccounts();
    }
  }

  return (
    <>
      <TitlePage title="Contas a Pagar">
        <Button
          type="button"
          variant="primary"
          className="!w-auto px-4 py-2"
          onClick={() => setModalOpen(true)}
        >
          Novo
        </Button>
      </TitlePage>

      <AccountPayableModal
        open={modalOpen}
        setOpen={(open) => {
          setModalOpen(open);
          if (!open) setEditingAccount(null);
        }}
        onCreated={reloadAccounts}
        editingAccount={editingAccount}
      />

      <div className="overflow-x-auto bg-white rounded-lg shadow">
        <AccountPayableList
          accounts={accounts}
          onEdit={(account) => {
            setEditingAccount(account);
            setModalOpen(true);
          }}
          onDelete={handleDelete}
          onMarkAsPaid={handleMarkAsPaid}
        />
      </div>
    </>
  );
};
