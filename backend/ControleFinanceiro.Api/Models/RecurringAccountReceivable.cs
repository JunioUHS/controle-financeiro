using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.Models
{
    public class RecurringAccountReceivable
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; } // null = recorr�ncia indeterminada

        [Required]
        public string Period { get; set; } = "Monthly"; // "Monthly", "Weekly", "Daily"

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}