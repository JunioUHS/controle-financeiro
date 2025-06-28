namespace ControleFinanceiro.Api.DTOs.CreditCardPurchase
{
    public class PurchaseInstallmentDto
    {
        public int Id { get; set; }
        public int NumberInstallment { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Value { get; set; }
        public bool IsPaid { get; set; }
    }
}