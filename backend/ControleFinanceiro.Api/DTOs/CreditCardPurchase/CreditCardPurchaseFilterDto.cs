namespace ControleFinanceiro.Api.DTOs.CreditCardPurchase
{
    public class CreditCardPurchaseFilterDto
    {
        public int? CreditCardId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public int Page { get; set; } = 1;
        //public int PageSize { get; set; } = 10;
    }
}