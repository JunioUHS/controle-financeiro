using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.CreditCardPurchase
{
    public class CreditCardPurchaseCreateDto
    {
        [Required(ErrorMessage = "Descri��o � obrigat�ria")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Valor � obrigat�rio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser positivo.")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "Data da compra � obrigat�ria")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "N�mero de parcelas � obrigat�rio")]
        [Range(1, 36, ErrorMessage = "O n�mero de parcelas deve ser entre 1 e 36.")]
        public int NumberInstallments { get; set; }

        [Required(ErrorMessage = "Categoria � obrigat�ria")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Cart�o � obrigat�rio")]
        public int? CreditCardId { get; set; }
    }
}