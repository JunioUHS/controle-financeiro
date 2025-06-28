import { createContext } from "react";
import type { AuthContextType } from "../types/context";

export const AuthContext = createContext<AuthContextType | undefined>(
  undefined
);
