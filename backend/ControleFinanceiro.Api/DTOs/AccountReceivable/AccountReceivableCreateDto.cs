using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.AccountReceivable
{
    public class AccountReceivableCreateDto
    {
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser positivo.")]
        public decimal Value { get; set; }

        [Required]
        public DateTime ReceiptDate { get; set; }

        [Required(ErrorMessage = "Categoria � obrigat�ria.")]
        public int? CategoryId { get; set; }
    }
}