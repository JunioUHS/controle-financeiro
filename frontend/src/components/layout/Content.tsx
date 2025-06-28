import React from "react";

interface ContentProps {
  children: React.ReactNode;
  className?: string;
}

export const Content: React.FC<ContentProps> = ({
  children,
  className = "",
}) => (
  <main className={`max-w-7xl mx-auto w-full px-4 py-8 ${className}`}>
    {children}
  </main>
);
