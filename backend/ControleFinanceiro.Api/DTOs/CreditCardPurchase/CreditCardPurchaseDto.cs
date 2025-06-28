using ControleFinanceiro.Api.DTOs.Category;
using ControleFinanceiro.Api.DTOs.CreditCard;
using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.CreditCardPurchase
{
    public class CreditCardPurchaseDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Value { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int NumberInstallments { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; } = null!;
        public int CreditCardId { get; set; }
        public CreditCardDto CreditCard { get; set; } = null!;
        public IEnumerable<PurchaseInstallmentDto> Installments { get; set; } = null!;
    }
}