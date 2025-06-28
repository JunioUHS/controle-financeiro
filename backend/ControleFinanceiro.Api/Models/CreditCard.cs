namespace ControleFinanceiro.Api.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Limit { get; set; }
        public decimal CurrentBalance { get; set; }

        public required string UserId { get; set; }
        public required ApplicationUser User { get; set; }

        public ICollection<CreditCardPurchase> Purchases { get; set; } = new List<CreditCardPurchase>();
    }
}
