import React, { useEffect, useState } from "react";
import { TitlePage } from "@/components/layout/TitlePage";
import { useApiToast } from "@/hooks/useApiToast";
import type { RecurringAccountReceivable } from "@/types/recurringAccountReceivable";
import { recurringAccountReceivableService } from "@/services/recurringAccountReceivableService";
import { RecurringAccountReceivableList } from "./conponents/RecurringAccountReceivableList";

export const RecurringAccountReceivablePage: React.FC = () => {
  const [accounts, setAccounts] = useState<RecurringAccountReceivable[]>([]);
  const { handleApiResponse } = useApiToast();

  async function reloadAccounts() {
    const res = await recurringAccountReceivableService.getAll();
    if (res.success) setAccounts(res.data ?? []);
  }

  useEffect(() => {
    reloadAccounts();
  }, []);

  async function handleDelete(id: number) {
    const res = await recurringAccountReceivableService.delete(id);
    const ok = await handleApiResponse(res, "Conta excluída com sucesso!");
    if (ok) reloadAccounts();
  }

  async function handleInactivate(id: number) {
    const res = await recurringAccountReceivableService.inactivate(id);
    const ok = await handleApiResponse(res, "Conta inativada com sucesso!");
    if (ok) reloadAccounts();
  }

  return (
    <>
      <TitlePage title="Configurar Recorrência de Contas a Receber" />

      <div className="overflow-x-auto bg-white rounded-lg shadow">
        <RecurringAccountReceivableList
          accounts={accounts}
          onDelete={handleDelete}
          onInactivate={handleInactivate}
        />
      </div>
    </>
  );
};
