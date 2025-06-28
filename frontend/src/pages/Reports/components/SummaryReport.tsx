import { formatMoney } from "@/helpers/moneyHelper";
import type { BalanceSummary } from "@/types/report";
import React from "react";

interface SummaryReportProps {
  loading: boolean;
  summary: BalanceSummary;
}

export const SummaryReport: React.FC<SummaryReportProps> = ({
  loading,
  summary,
}) => (
  <>
    {loading ? (
      <div>Carregando...</div>
    ) : summary ? (
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mt-4">
        <div className="bg-white p-4 rounded shadow">
          <div className="text-gray-500">Receita Total</div>
          <div className="text-xl font-bold text-green-600">
            {formatMoney(summary.totalReceivable)}
          </div>
        </div>
        <div className="bg-white p-4 rounded shadow">
          <div className="text-gray-500">Despesa Total</div>
          <div className="text-xl font-bold text-red-600">
            {formatMoney(summary.totalPayable)}
          </div>
        </div>
        <div className="bg-white p-4 rounded shadow">
          <div className="text-gray-500">Saldo</div>
          <div className="text-xl font-bold text-blue-600">
            {formatMoney(summary.balance)}
          </div>
        </div>
      </div>
    ) : (
      <div>Nenhum dado encontrado.</div>
    )}
  </>
);
