import React, { useEffect, useState } from "react";
import { Button } from "@/components/custom-ui/Button";
import { TitlePage } from "@/components/layout/TitlePage";
import { useApiToast } from "@/hooks/useApiToast";
import { accountReceivableService } from "@/services/accountReceivableService";
import type { AccountReceivable } from "@/types/accountReceivable";
import { AccountReceivableList } from "./components/AccountReceivableList";
import { AccountReceivableModal } from "./components/AccountPayableModal";

export const AccountReceivablePage: React.FC = () => {
  const [accounts, setAccounts] = useState<AccountReceivable[]>([]);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingAccount, setEditingAccount] =
    useState<AccountReceivable | null>(null);
  const { handleApiResponse } = useApiToast();

  async function reloadAccounts() {
    const res = await accountReceivableService.getAll();
    if (res.success) setAccounts(res.data ?? []);
  }

  useEffect(() => {
    reloadAccounts();
  }, []);

  async function handleDelete(id: number) {
    const res = await accountReceivableService.delete(id);
    const ok = await handleApiResponse(res, "Conta excluída com sucesso!");
    if (ok) reloadAccounts();
  }

  async function handleMarkAsReceived(id: number) {
    const res = await accountReceivableService.markAsReceived(id);
    const ok = await handleApiResponse(res, "Conta marcada como recebida!");
    if (ok) reloadAccounts();
  }

  return (
    <>
      <TitlePage title="Contas a Receber">
        <Button
          type="button"
          variant="primary"
          className="!w-auto px-4 py-2"
          onClick={() => setModalOpen(true)}
        >
          Nova
        </Button>
      </TitlePage>

      <AccountReceivableModal
        open={modalOpen}
        setOpen={(open) => {
          setModalOpen(open);
          if (!open) setEditingAccount(null);
        }}
        onCreated={reloadAccounts}
        editingAccount={editingAccount}
      />

      <div className="overflow-x-auto bg-white rounded-lg shadow">
        <AccountReceivableList
          accounts={accounts}
          onEdit={(account) => {
            setEditingAccount(account);
            setModalOpen(true);
          }}
          onDelete={handleDelete}
          onMarkAsReceived={handleMarkAsReceived}
        />
      </div>
    </>
  );
};
