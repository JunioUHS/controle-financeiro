using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.CreditCard
{
    public class CreditCardDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome � obrigat�rio")]
        public required string Name { get; set; }
        public decimal Limit { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal AvailableBalance => Limit - CurrentBalance;
    }
}