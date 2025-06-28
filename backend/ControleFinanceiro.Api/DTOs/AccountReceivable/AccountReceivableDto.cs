using ControleFinanceiro.Api.DTOs.Category;
using System;

namespace ControleFinanceiro.Api.DTOs.AccountReceivable
{
    public class AccountReceivableDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime ReceiptDate { get; set; }
        public bool IsReceived { get; set; }
        public CategoryDto Category { get; set; } = null!;
    }
}