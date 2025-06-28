namespace ControleFinanceiro.Api.DTOs.Reports
{
    public class CreditCardTransactionReportDto
    {
        public string CardName { get; set; } = null!;
        public DateTime PurchaseDate { get; set; }
        public string Description { get; set; } = null!;
        public decimal Value { get; set; }
        public string Category { get; set; } = null!;
    }
}