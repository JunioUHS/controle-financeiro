import React from "react";

export interface InputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
}

export const Input: React.FC<InputProps> = ({
  label,
  error,
  className = "",
  ...props
}) => {
  return (
    <div className="mb-4">
      <label className="block text-sm font-medium text-gray-700 mb-2">
        {label}
      </label>
      <input
        className={`
          w-full px-3 py-2 border border-gray-300 rounded-md 
          focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent
          ${error ? "border-red-500" : ""}
          ${className}
        `}
        {...props}
      />
      {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
    </div>
  );
};
