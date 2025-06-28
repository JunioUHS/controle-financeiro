using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Models
{
    [Index(nameof(Name), nameof(UserId), IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public required string Name { get; set; }
        [Required]
        public required string UserId { get; set; }
        public required ApplicationUser User { get; set; }
        public ICollection<AccountPayable> AccountsPayable { get; set; } = new List<AccountPayable>();
        public ICollection<AccountReceivable> AccountsReceivable { get; set; } = new List<AccountReceivable>();
        public ICollection<CreditCardPurchase> CreditCardPurchases { get; set; } = new List<CreditCardPurchase>();
        public ICollection<RecurringAccountReceivable> RecurringAccountsReceivable { get; set; } = new List<RecurringAccountReceivable>();
    }
}
