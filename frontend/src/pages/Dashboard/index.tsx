import { useEffect, useState } from "react";
import { TitlePage } from "../../components/layout/TitlePage";
import { useAuth } from "../../hooks/useAuth";
import { reportService } from "@/services/reportService";
import type { BalanceSummary } from "@/types/report";
import { formatMoney } from "@/helpers/moneyHelper";

export default function Dashboard() {
  const { user } = useAuth();
  const [summary, setSummary] = useState<BalanceSummary>();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const end = new Date();
    const start = new Date();
    start.setDate(end.getDate() - 30);

    reportService
      .getBalanceSummary(start.toISOString(), end.toISOString())
      .then((res) => {
        if (res.success) setSummary(res.data);
      })
      .finally(() => setLoading(false));
  }, []);

  return (
    <>
      <TitlePage title="Dashboard" />

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="p-5">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className="w-8 h-8 bg-green-500 rounded-md flex items-center justify-center">
                  <span className="text-white font-bold">$</span>
                </div>
              </div>
              <div className="ml-5 w-0 flex-1">
                <dl>
                  <dt className="text-sm font-medium text-gray-500 truncate">
                    Receita Total
                  </dt>
                  <dd className="text-lg font-medium text-gray-900">
                    {loading
                      ? "Carregando..."
                      : summary
                      ? formatMoney(summary.totalReceivable)
                      : "--"}
                  </dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="p-5">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className="w-8 h-8 bg-red-500 rounded-md flex items-center justify-center">
                  <span className="text-white font-bold">-</span>
                </div>
              </div>
              <div className="ml-5 w-0 flex-1">
                <dl>
                  <dt className="text-sm font-medium text-gray-500 truncate">
                    Despesa Total
                  </dt>
                  <dd className="text-lg font-medium text-gray-900">
                    {loading
                      ? "Carregando..."
                      : summary
                      ? formatMoney(summary.totalPayable)
                      : "--"}
                  </dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="p-5">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className="w-8 h-8 bg-blue-500 rounded-md flex items-center justify-center">
                  <span className="text-white font-bold">Σ</span>
                </div>
              </div>
              <div className="ml-5 w-0 flex-1">
                <dl>
                  <dt className="text-sm font-medium text-gray-500 truncate">
                    Saldo
                  </dt>
                  <dd className="text-lg font-medium text-gray-900">
                    {loading
                      ? "Carregando..."
                      : summary
                      ? formatMoney(summary.balance)
                      : "--"}
                  </dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Seção de informações do usuário */}
      <div className="bg-white shadow rounded-lg">
        <div className="px-4 py-5 sm:p-6">
          <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
            Suas Informações
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {/* <div>
              <label className="block text-sm font-medium text-gray-700">
                ID do Usuário
              </label>
              <p className="mt-1 text-sm text-gray-900">{user?.id}</p>
            </div> */}
            <div>
              <label className="block text-sm font-medium text-gray-700">
                Nome de Usuário
              </label>
              <p className="mt-1 text-sm text-gray-900">{user?.userName}</p>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">
                Nome Completo
              </label>
              <p className="mt-1 text-sm text-gray-900">{user?.fullName}</p>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
