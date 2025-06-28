// components/GlobalLoading.tsx
import { useNavigation } from "react-router-dom";
import { Loading } from "./Loading";

export function GlobalLoading() {
  const navigation = useNavigation();

  // Pode ser 'idle' | 'submitting' | 'loading'
  if (navigation.state === "loading") {
    console.log("Carregando dados global...");
    return <Loading message="Carregando dados..." />;
  }

  return null;
}
