import React, { useState } from "react";
import { Button } from "@/components/custom-ui/Button";
import { TitlePage } from "@/components/layout/TitlePage";
import { useLoaderData } from "react-router-dom";
import type { CreditCard } from "@/types/creditCard";
import { creditCardService } from "@/services/creditCardService";
import { CreditCardList } from "./components/CreditCardList";
import { CreditCardModal } from "./components/CategoryModal";
export const CreditCardPage: React.FC = () => {
  const initialCreditCards = useLoaderData() as CreditCard[];
  const [creditCards, setCreditCards] = useState(initialCreditCards);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingCreditCard, setEditingCreditCard] = useState<CreditCard | null>(
    null
  );

  async function reloadCreditCards() {
    const res = await creditCardService.getAll();
    if (res.success) setCreditCards(res.data ?? []);
  }

  return (
    <>
      <TitlePage title="Cartões de Crédito">
        <Button
          type="button"
          variant="primary"
          className="!w-auto px-4 py-2"
          onClick={() => setModalOpen(true)}
        >
          Novo
        </Button>
      </TitlePage>

      <CreditCardModal
        open={modalOpen}
        setOpen={(open) => {
          setModalOpen(open);
          if (!open) setEditingCreditCard(null);
        }}
        onCreated={reloadCreditCards}
        editingCreditCard={editingCreditCard}
      />

      <div className="overflow-x-auto bg-white rounded-lg shadow">
        <CreditCardList
          creditCards={creditCards}
          onEdit={(creditCard) => {
            setEditingCreditCard(creditCard);
            setModalOpen(true);
          }}
        />
      </div>
    </>
  );
};
