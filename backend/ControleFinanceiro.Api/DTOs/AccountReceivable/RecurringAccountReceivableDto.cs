using ControleFinanceiro.Api.DTOs.Category;
using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.AccountReceivable
{
    public class RecurringAccountReceivableDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Descri��o � obrigat�ria")]
        public string Description { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; } = null!;
    }
}