using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.AccountReceivable
{
    public class RecurringAccountReceivableCreateDto
    {
        [Required(ErrorMessage = "Descri��o � obrigat�ria")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser positivo.")]
        public decimal Value { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //[Required]
        //public string Period { get; set; } = "Monthly";

        [Required]
        public int? CategoryId { get; set; }
    }
}