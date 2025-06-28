export function formatMoney(value: number | string): string {
    if (value === null || value === undefined || isNaN(Number(value))) return "R$ 0,00";
    return `R$ ${Number(value).toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    })}`;
}