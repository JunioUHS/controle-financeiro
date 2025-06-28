namespace ControleFinanceiro.Api.Models
{
    public class AccountReceivable
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime ReceiptDate { get; set; }
        public bool IsReceived { get; set; }

        public int CategoryId { get; set; }
        public required Category Category { get; set; }

        public required string UserId { get; set; }
        public required ApplicationUser User { get; set; }

        public int? RecurringAccountReceivableId { get; set; }
        public RecurringAccountReceivable? RecurringAccountReceivable { get; set; }
    }
}
