import React from "react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
  DialogClose,
} from "@/components/ui/dialog";

interface ModalBaseProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  title: string;
  children: React.ReactNode;
  footer?: React.ReactNode;
  maxWidth?: string; // ex: "max-w-7xl"
}

export const ModalBase: React.FC<ModalBaseProps> = ({
  open,
  onOpenChange,
  title,
  children,
  footer,
  maxWidth = "max-w-5xl",
}) => (
  <Dialog open={open} onOpenChange={onOpenChange}>
    <DialogContent
      className={`w-full ${maxWidth} max-h-[80vh] flex flex-col`}
      onInteractOutside={(e) => e.preventDefault()}
    >
      <DialogHeader>
        <DialogTitle>{title}</DialogTitle>
      </DialogHeader>
      <DialogClose asChild>
        <button
          type="button"
          className="absolute right-4 top-4 text-lg"
          aria-label="Fechar"
        ></button>
      </DialogClose>
      <div className="border-b border-gray-200 mb-1" />
      <div
        className="overflow-y-auto flex-1 px-2"
        style={{ maxHeight: "60vh" }}
      >
        {children}
      </div>
      {footer && (
        <>
          <div className="border-b border-gray-200 mb-1" />
          <DialogFooter>{footer}</DialogFooter>
        </>
      )}
    </DialogContent>
  </Dialog>
);
