import React, { useState } from "react";
import { Button } from "@/components/custom-ui/Button";
import { TitlePage } from "@/components/layout/TitlePage";
import { useLoaderData } from "react-router-dom";
import { useApiToast } from "@/hooks/useApiToast";
import type {
  CreditCardPurchase,
  CreditCardPurchaseFilter,
} from "@/types/creditCardPurchase";
import { creditCardPurchaseService } from "@/services/creditCardPurchaseService";
import { CreditCardPurchaseList } from "./components/CreditCardPurchaseList";
import { CreditCardPurchaseModal } from "./components/CreditCardPurchaseModal";
import { InstallmentsModal } from "./components/InstallmentsModal";

export const CreditCardPurchasePage: React.FC = () => {
  const { creditCardId, initialCreditCardPurchases } = useLoaderData() as {
    creditCardId: number;
    initialCreditCardPurchases: CreditCardPurchase[];
  };
  const [creditCardPurchases, setCreditCardPurchases] = useState(
    initialCreditCardPurchases
  );
  const [modalOpen, setModalOpen] = useState(false);
  const [installmentsModalOpen, setInstallmentsModalOpen] = useState(false);
  const [selectedPurchase, setSelectedPurchase] =
    useState<CreditCardPurchase | null>(null);
  const { handleApiResponse } = useApiToast();

  async function reloadCreditCardPurchases() {
    const queryParams: CreditCardPurchaseFilter = {
      creditCardId: creditCardId,
    };

    const res = await creditCardPurchaseService.getAll(queryParams);
    if (res.success) setCreditCardPurchases(res.data ?? []);
  }

  async function handleDelete(id: number) {
    const res = await creditCardPurchaseService.delete(id);
    const ok = await handleApiResponse(res, "Compra excluída com sucesso!");
    if (ok) {
      reloadCreditCardPurchases();
    }
  }

  async function handleMarkInstallmentAsPaid(installmentId: number) {
    const res = await creditCardPurchaseService.markInstallmentAsPaid(
      installmentId
    );
    const ok = await handleApiResponse(res, "Parcela paga!");
    if (ok) {
      reloadCreditCardPurchases();

      if (selectedPurchase) {
        const updated = await creditCardPurchaseService.getById(
          selectedPurchase.id
        );
        if (updated.success && updated.data) {
          setSelectedPurchase(updated.data);
        }
      }
    }
  }

  return (
    <>
      <TitlePage title="Compras do Cartão de Crédito">
        <Button
          type="button"
          variant="primary"
          className="!w-auto px-4 py-2"
          onClick={() => setModalOpen(true)}
        >
          Novo
        </Button>
      </TitlePage>
      <CreditCardPurchaseModal
        open={modalOpen}
        setOpen={(open) => {
          setModalOpen(open);
        }}
        onCreated={reloadCreditCardPurchases}
        creditCardId={creditCardId}
      />

      <InstallmentsModal
        open={installmentsModalOpen}
        setOpen={setInstallmentsModalOpen}
        purchase={selectedPurchase}
        onMarkInstallmentAsPaid={handleMarkInstallmentAsPaid}
      />

      <div className="overflow-x-auto bg-white rounded-lg shadow">
        <CreditCardPurchaseList
          creditCardPurchases={creditCardPurchases}
          onDelete={handleDelete}
          onShowInstallments={(purchase) => {
            setSelectedPurchase(purchase);
            setInstallmentsModalOpen(true);
          }}
        />
      </div>
    </>
  );
};
