// App.tsx
import { RouterProvider } from "react-router-dom";
import { AuthProvider } from "./providers/AuthProvider";
import { router } from "./router";
import { Toaster } from "sonner";
import { Loading } from "./components";

function App() {
  return (
    <AuthProvider>
      <RouterProvider
        router={router}
        fallbackElement={<Loading message="Carregando..." />}
      />
      {/* Toast notifications */}
      <Toaster position="bottom-right" richColors closeButton duration={5000} />
    </AuthProvider>
  );
}

export default App;
