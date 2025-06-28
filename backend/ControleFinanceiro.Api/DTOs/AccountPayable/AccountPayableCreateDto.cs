using System;
using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.AccountPayable
{
    public class AccountPayableCreateDto
    {
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser positivo.")]
        public decimal Value { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Categoria � obrigat�ria.")]
        public int? CategoryId { get; set; }
    }
}