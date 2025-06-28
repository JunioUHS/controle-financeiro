import React from "react";

interface ForgotPasswordLinkProps {
  onClick?: () => void;
}

export const ForgotPasswordLink: React.FC<ForgotPasswordLinkProps> = ({
  onClick,
}) => {
  return (
    <div className="text-sm">
      <button
        type="button"
        onClick={onClick}
        className="font-medium text-blue-600 hover:text-blue-500 transition-colors"
      >
        Esqueceu a senha?
      </button>
    </div>
  );
};
