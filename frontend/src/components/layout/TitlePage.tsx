import React from "react";

interface TitlePageProps {
  title: string;
  children?: React.ReactNode;
  className?: string;
}

export const TitlePage: React.FC<TitlePageProps> = ({
  title,
  children,
  className = "",
}) => (
  <div className={`flex items-center justify-between mb-6 ${className}`}>
    <h1 className="text-3xl font-bold text-gray-900">{title}</h1>
    {children}
  </div>
);
