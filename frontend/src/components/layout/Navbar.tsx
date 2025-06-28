import React from "react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { Button } from "../custom-ui/Button";
import { Bell } from "lucide-react";
import {
  NavigationMenu,
  NavigationMenuList,
  NavigationMenuItem,
  NavigationMenuLink,
} from "@/components/ui/navigation-menu";
import {
  Popover,
  PopoverTrigger,
  PopoverContent,
} from "@/components/ui/popover";

export const Navbar: React.FC = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = async () => {
    await logout();
    navigate("/login", { replace: true });
  };

  const isActive = (path: string) => location.pathname.startsWith(path);

  return (
    <nav className="bg-blue-600 text-white shadow mb-6">
      <div className="max-w-7xl mx-auto px-4 py-3 flex items-center justify-between">
        {/* NavigationMenu para os links principais */}
        <NavigationMenu>
          <NavigationMenuList className="flex items-center gap-6">
            <NavigationMenuItem>
              <NavigationMenuLink asChild>
                <Link
                  to="/dashboard"
                  className={`font-semibold text-white hover:text-white focus:text-white active:text-white hover:underline transition-colors hover:bg-blue-500 focus:bg-blue-500 active:bg-blue-500 ${
                    isActive("/dashboard") ? "underline" : ""
                  }`}
                >
                  Dashboard
                </Link>
              </NavigationMenuLink>
            </NavigationMenuItem>
            <NavigationMenuItem>
              <NavigationMenuLink asChild>
                <Link
                  to="/accounts-payable"
                  className={`font-semibold text-white hover:text-white focus:text-white active:text-white hover:underline transition-colors hover:bg-blue-500 focus:bg-blue-500 active:bg-blue-500 ${
                    isActive("/account-payable") ? "underline" : ""
                  }`}
                >
                  Contas a Pagar
                </Link>
              </NavigationMenuLink>
            </NavigationMenuItem>
            {/* Popover para Contas a Receber e Recorrência */}
            <NavigationMenuItem>
              <Popover>
                <PopoverTrigger asChild>
                  <button
                    className="font-semibold text-white hover:text-white focus:text-white active:text-white hover:underline transition-colors hover:bg-blue-500 focus:bg-blue-500 active:bg-blue-500 px-2 py-1 rounded"
                    type="button"
                  >
                    Recebíveis
                  </button>
                </PopoverTrigger>
                <PopoverContent className="w-48 p-2 flex flex-col gap-1 bg-white border border-gray-200 shadow-lg rounded-lg">
                  <Link
                    to="/accounts-receivable"
                    className={`block px-4 py-2 rounded hover:bg-blue-100 text-blue-700 font-medium transition-colors ${
                      isActive("/account-receivable") ? "bg-blue-100" : ""
                    }`}
                  >
                    Contas a Receber
                  </Link>
                  <Link
                    to="/recurring-accounts-receivable"
                    className={`block px-4 py-2 rounded hover:bg-blue-100 text-blue-700 font-medium transition-colors ${
                      isActive("/recurring-account-receivable")
                        ? "bg-blue-100"
                        : ""
                    }`}
                  >
                    Recorrência
                  </Link>
                </PopoverContent>
              </Popover>
            </NavigationMenuItem>
            <NavigationMenuItem>
              <NavigationMenuLink asChild>
                <Link
                  to="/credit-cards"
                  className={`font-semibold text-white hover:text-white focus:text-white active:text-white hover:underline transition-colors hover:bg-blue-500 focus:bg-blue-500 active:bg-blue-500 ${
                    isActive("/credit-cards") ? "underline" : ""
                  }`}
                >
                  Cartão de Crédito
                </Link>
              </NavigationMenuLink>
            </NavigationMenuItem>
            <NavigationMenuItem>
              <NavigationMenuLink asChild>
                <Link
                  to="/categories"
                  className={`font-semibold text-white hover:text-white focus:text-white active:text-white hover:underline transition-colors hover:bg-blue-500 focus:bg-blue-500 active:bg-blue-500 ${
                    isActive("/categories") ? "underline" : ""
                  }`}
                >
                  Categorias
                </Link>
              </NavigationMenuLink>
            </NavigationMenuItem>
            <NavigationMenuItem>
              <NavigationMenuLink asChild>
                <Link
                  to="/reports"
                  className={`font-semibold text-white hover:text-white focus:text-white active:text-white hover:underline transition-colors hover:bg-blue-500 focus:bg-blue-500 active:bg-blue-500 ${
                    isActive("/reports") ? "underline" : ""
                  }`}
                >
                  Relatórios
                </Link>
              </NavigationMenuLink>
            </NavigationMenuItem>
          </NavigationMenuList>
        </NavigationMenu>

        <div className="flex items-center gap-4">
          {/* Ícone de notificações */}
          <button
            type="button"
            className="relative p-2 rounded-full hover:bg-blue-500 transition-colors"
            aria-label="Notificações"
          >
            <Bell className="w-5 h-5 text-white" />
          </button>
          {/* Botão Sair */}
          <Button
            type="button"
            variant="secondary"
            className="px-4 py-2 rounded text-red-600 bg-white hover:bg-red-100 font-medium transition-colors"
            onClick={handleLogout}
            aria-label="Sair"
          >
            Sair
          </Button>
        </div>
      </div>
    </nav>
  );
};
