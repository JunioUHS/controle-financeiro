using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.CreditCard
{
    public class CreditCardCreateDto
    {
        [Required(ErrorMessage = "Nome � obrigat�rio")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Limite � obrigat�rio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O limite deve ser positivo.")]
        public decimal Limit { get; set; }
    }
}