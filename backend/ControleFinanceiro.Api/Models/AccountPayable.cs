namespace ControleFinanceiro.Api.Models
{
    public class AccountPayable
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }

        public int CategoryId { get; set; }
        public required Category Category { get; set; }

        public required string UserId { get; set; }
        public required ApplicationUser User { get; set; }
    }
}
