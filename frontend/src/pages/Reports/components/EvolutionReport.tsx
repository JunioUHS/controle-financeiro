import React from "react";
import { format } from "date-fns";
import type { BalanceEvolution } from "@/types/report";
// Instale: npm install recharts
import {
  ResponsiveContainer,
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
} from "recharts";

interface EvolutionReportProps {
  loading: boolean;
  evolution: BalanceEvolution[];
}

export const EvolutionReport: React.FC<EvolutionReportProps> = ({
  loading,
  evolution,
}) => (
  <>
    {loading ? (
      <div>Carregando...</div>
    ) : evolution.length > 0 ? (
      <div className="bg-white rounded shadow mt-4 p-4">
        <ResponsiveContainer width="100%" height={300}>
          <LineChart data={evolution}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
              dataKey="date"
              tickFormatter={(date) => format(new Date(date), "dd/MM")}
            />
            <YAxis
              tickFormatter={(value) =>
                value.toLocaleString("pt-BR", { minimumFractionDigits: 2 })
              }
            />
            <Tooltip
              labelFormatter={(date) =>
                `Data: ${format(new Date(date), "dd/MM/yyyy")}`
              }
              formatter={(value: number) =>
                `R$ ${value.toLocaleString("pt-BR", {
                  minimumFractionDigits: 2,
                })}`
              }
            />
            <Line
              type="monotone"
              dataKey="balance"
              stroke="#2563eb"
              strokeWidth={2}
              dot={false}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>
    ) : (
      <div>Nenhum dado encontrado.</div>
    )}
  </>
);
