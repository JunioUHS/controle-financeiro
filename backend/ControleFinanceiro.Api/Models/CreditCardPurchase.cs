namespace ControleFinanceiro.Api.Models
{
    public class CreditCardPurchase
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int NumberInstallments { get; set; } = 1;

        public ICollection<PurchaseInstallment> Installments { get; set; } = new List<PurchaseInstallment>();

        public int CategoryId { get; set; }
        public required Category Category { get; set; }

        public int CreditCardId { get; set; }
        public required CreditCard CreditCard { get; set; }
    }
}
