import * as React from "react";
import { ChevronsUpDown, Check } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import {
  Popover,
  PopoverTrigger,
  PopoverContent,
} from "@/components/ui/popover";
import {
  Command,
  CommandInput,
  CommandList,
  CommandEmpty,
  CommandGroup,
  CommandItem,
} from "@/components/ui/command";

interface ComboboxOption {
  value: string | number;
  label: string;
}

interface ComboboxProps {
  options: ComboboxOption[];
  value: string | number | null;
  onChange: (value: string | number | null) => void;
  placeholder?: string;
  error?: string;
  className?: string;
}

export const Combobox: React.FC<ComboboxProps> = ({
  options,
  value,
  onChange,
  placeholder = "Selecione...",
  error,
  className,
}) => {
  const [open, setOpen] = React.useState(false);
  const selected = options.find((opt) => opt.value === value);

  return (
    <div className={className}>
      <Popover open={open} onOpenChange={setOpen}>
        <PopoverTrigger asChild>
          <Button
            variant="outline"
            role="combobox"
            aria-expanded={open}
            className={cn(
              "w-full justify-between",
              error ? "border-red-500" : ""
            )}
            type="button"
          >
            {selected ? selected.label : placeholder}
            <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
          </Button>
        </PopoverTrigger>
        <PopoverContent className="w-full p-0">
          <Command>
            <CommandInput placeholder="Buscar..." className="h-9" />
            <CommandList>
              <CommandEmpty>Nenhuma opção encontrada.</CommandEmpty>
              <CommandGroup>
                {options.map((opt) => (
                  <CommandItem
                    key={opt.value}
                    value={String(opt.value)}
                    onSelect={() => {
                      onChange(opt.value);
                      setOpen(false);
                    }}
                  >
                    {opt.label}
                    <Check
                      className={cn(
                        "ml-auto h-4 w-4",
                        value === opt.value ? "opacity-100" : "opacity-0"
                      )}
                    />
                  </CommandItem>
                ))}
              </CommandGroup>
            </CommandList>
          </Command>
        </PopoverContent>
      </Popover>
      {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
    </div>
  );
};
