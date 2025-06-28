using ControleFinanceiro.Api.DTOs.Category;

namespace ControleFinanceiro.Api.DTOs.AccountPayable
{
    public class AccountPayableDto
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; } = null!;
    }
}
