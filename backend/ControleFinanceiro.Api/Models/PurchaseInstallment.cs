namespace ControleFinanceiro.Api.Models
{
    public class PurchaseInstallment
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        public required CreditCardPurchase Purchase { get; set; }

        public int NumberInstallment { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Value { get; set; }
        public bool IsPaid { get; set; }
    }
}
