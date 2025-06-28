import { createBrowserRouter, Navigate } from "react-router-dom";
import ProtectedLayout from "./layouts/ProtectedLayout";
import { protectedLayoutLoader } from "./loaders/protectedLayoutLoader";
import Dashboard from "./pages/Dashboard";
import { Login } from "./pages/Login";
import RootLayout from "./layouts/RootLayout";
import { AccountPayablePage } from "./pages/AccountPayable";
import { accountPayableLoader } from "./loaders/accountPayableLoader";
import { CategoryPage } from "./pages/Category";
import { categoryLoader } from "./loaders/categoryLoader";
import { CreditCardPage } from "./pages/CreditCard";
import { creditCardLoader } from "./loaders/creditCardLoader";
import { CreditCardPurchasePage } from "./pages/CreditCardPurchase";
import { creditCardPurchaseLoader } from "./loaders/creditCardPurchaseLoader";
import { AccountReceivablePage } from "./pages/AccountReceivable";
import { accountReceivableLoader } from "./loaders/accountReceivableLoader";
import { RecurringAccountReceivablePage } from "./pages/RecurringAccountReceivable";
import { recurringAccountReceivableLoader } from "./loaders/recurringAccountReceivableLoader";
import { Register } from "./pages/Register";
import { ReportsPage } from "./pages/Reports";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <RootLayout />,
    children: [
      {
        path: "/login",
        element: <Login />,
      },
      {
        path: "/register",
        element: <Register />,
      },
      {
        element: <ProtectedLayout />,
        loader: protectedLayoutLoader,
        children: [
          {
            index: true,
            element: <Navigate to="/dashboard" replace />,
          },
          {
            path: "dashboard",
            element: <Dashboard />,
          },
          {
            path: "accounts-payable",
            element: <AccountPayablePage />,
            loader: accountPayableLoader,
          },
          {
            path: "accounts-receivable",
            element: <AccountReceivablePage />,
            loader: accountReceivableLoader,
          },
          {
            path: "recurring-accounts-receivable",
            element: <RecurringAccountReceivablePage />,
            loader: recurringAccountReceivableLoader,
          },
          {
            path: "categories",
            element: <CategoryPage />,
            loader: categoryLoader,
          },
          {
            path: "credit-cards",
            element: <CreditCardPage />,
            loader: creditCardLoader,
          },
          {
            path: "credit-cards/:id/purchases",
            element: <CreditCardPurchasePage />,
            loader: creditCardPurchaseLoader,
          },
          {
            path: "reports",
            element: <ReportsPage />,
          },
        ],
      },
      {
        path: "*",
        element: <Navigate to="/dashboard" />,
      },
    ],
  },
]);
