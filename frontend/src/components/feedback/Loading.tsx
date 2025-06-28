import React from "react";

interface LoadingProps {
  message?: string;
  isGlobal?: boolean;
}

export const Loading: React.FC<LoadingProps> = ({
  message = "Carregando...",
  isGlobal = false,
}) => {
  // Se global, usa min-h-screen, senão min-h-[200px] (ou ajuste conforme o contexto)
  const containerClass = isGlobal
    ? "min-h-screen flex flex-col items-center justify-center bg-gray-50"
    : "min-h-[300px] flex flex-col items-center justify-center";

  console.log("Loading component rendered with message:", message);

  return (
    <div className={containerClass}>
      <div className="animate-spin rounded-full h-16 w-16 border-b-2 border-blue-600 mb-4"></div>
      <p className="text-gray-600 text-lg">{message}</p>
    </div>
  );
};
