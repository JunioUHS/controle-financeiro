import React, { useState, useEffect } from "react";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import { reportService } from "@/services/reportService";
import { creditCardService } from "@/services/creditCardService";
import { format } from "date-fns";
import { TitlePage } from "@/components/layout/TitlePage";
import { SummaryReport } from "./components/SummaryReport";
import { ExpensesByCategoryReport } from "./components/ExpensesByCategoryReport";
import { EvolutionReport } from "./components/EvolutionReport";
import { CreditCardTransactionsReport } from "./components/CreditCardTransactionsReport";
import { Combobox } from "@/components/custom-ui/Combobox";
import { Input } from "@/components/custom-ui/Input";
import type {
  BalanceEvolution,
  BalanceSummary,
  ExpensesByCategory,
  CreditCardTransaction,
} from "@/types/report";
import type { CreditCard } from "@/types/creditCard";

export const ReportsPage: React.FC = () => {
  const [activeTab, setActiveTab] = useState("resumo");
  const [summary, setSummary] = useState<BalanceSummary>({} as BalanceSummary);
  const [evolution, setEvolution] = useState<BalanceEvolution[]>([]);
  const [expensesByCategory, setExpensesByCategory] = useState<
    ExpensesByCategory[]
  >([]);
  const [transactions, setTransactions] = useState<CreditCardTransaction[]>([]);
  const [selectedCardId, setSelectedCardId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [creditCards, setCreditCards] = useState<CreditCard[]>([]);
  const [start, setStart] = useState(() => {
    const d = new Date();
    d.setDate(d.getDate() - 30);
    return d.toISOString().slice(0, 10);
  });
  const [end, setEnd] = useState(() => new Date().toISOString().slice(0, 10));

  // Mês atual para despesas por categoria
  const monthStr = format(new Date(), "yyyy-MM-01");

  useEffect(() => {
    setLoading(true);
    if (activeTab === "resumo") {
      reportService
        .getBalanceSummary(start, end)
        .then((res) => setSummary(res.data ?? ({} as BalanceSummary)))
        .finally(() => setLoading(false));
    }
    if (activeTab === "evolucao") {
      reportService
        .getBalanceEvolution(start, end)
        .then((res) => setEvolution(res.data ?? []))
        .finally(() => setLoading(false));
    }
    if (activeTab === "categoria") {
      reportService
        .getExpensesByCategory(start, end)
        .then((res) => setExpensesByCategory(res.data ?? []))
        .finally(() => setLoading(false));
    }
  }, [activeTab, start, end, monthStr]);

  useEffect(() => {
    if (activeTab === "cartao" && selectedCardId) {
      setLoading(true);
      reportService
        .getCreditCardTransactions(selectedCardId, start, end)
        .then((res) => setTransactions(res.data ?? []))
        .finally(() => setLoading(false));
    } else {
      setTransactions([]);
      setLoading(false);
    }
  }, [activeTab, selectedCardId, start, end]);

  useEffect(() => {
    if (activeTab === "cartao") {
      creditCardService.getAll().then((res) => {
        if (res.success) setCreditCards(res.data ?? []);
      });
    }
  }, [activeTab]);

  return (
    <>
      <TitlePage title="Relatórios" />

      {/* Filtros de período */}
      <div className="flex flex-wrap gap-4 mb-6 items-end">
        <div>
          <Input
            label="Data Inicial"
            type="date"
            className="border rounded px-2 py-1"
            value={start}
            onChange={(e) => setStart(e.target.value)}
            max={end}
          />
        </div>
        <div>
          <Input
            label="Data Final"
            type="date"
            className="border rounded px-2 py-1"
            value={end}
            onChange={(e) => setEnd(e.target.value)}
            min={start}
          />
        </div>
      </div>

      <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
        <TabsList>
          <TabsTrigger value="resumo">Resumo</TabsTrigger>
          <TabsTrigger value="categoria">Despesas por Categoria</TabsTrigger>
          <TabsTrigger value="evolucao">Evolução do Saldo</TabsTrigger>
          <TabsTrigger value="cartao">Transações Cartão</TabsTrigger>
        </TabsList>

        <TabsContent value="resumo">
          <SummaryReport loading={loading} summary={summary} />
        </TabsContent>

        <TabsContent value="categoria">
          <ExpensesByCategoryReport
            loading={loading}
            expensesByCategory={expensesByCategory}
          />
        </TabsContent>

        <TabsContent value="evolucao">
          <EvolutionReport loading={loading} evolution={evolution} />
        </TabsContent>

        <TabsContent value="cartao">
          <div className="mb-4">
            <label className="block mb-2">Selecione o cartão:</label>
            <Combobox
              options={creditCards.map((card) => ({
                value: card.id,
                label: card.name,
              }))}
              value={selectedCardId}
              onChange={(value) =>
                setSelectedCardId(value !== null ? Number(value) : null)
              }
              placeholder="Selecione..."
              className="w-64"
            />
          </div>
          {selectedCardId && (
            <CreditCardTransactionsReport
              loading={loading}
              transactions={transactions}
            />
          )}
        </TabsContent>
      </Tabs>
    </>
  );
};
