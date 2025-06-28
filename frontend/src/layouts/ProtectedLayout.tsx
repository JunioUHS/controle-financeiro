// layouts/ProtectedLayout.tsx
import { Outlet, useLoaderData, useNavigation } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { useEffect } from "react";
import type { User } from "../types";
import { Loading } from "../components";
import { Navbar } from "../components/layout/Navbar";
import { Content } from "../components/layout/Content";

export default function ProtectedLayout() {
  const loaderData = useLoaderData() as { user?: User };
  const { login, loading, isAuthenticated } = useAuth();
  const navigation = useNavigation();

  useEffect(() => {
    if (loaderData?.user) {
      login(loaderData.user);
    }
  }, [loaderData, login]);

  // Se não houver user, não renderiza nada (ou pode redirecionar)
  if (!isAuthenticated || !loaderData?.user) {
    return null;
  }

  return (
    <>
      <Navbar />
      <Content>
        {loading || navigation.state === "loading" ? (
          <Loading message="Carregando dados..." />
        ) : (
          <Outlet />
        )}
      </Content>
    </>
  );
}
