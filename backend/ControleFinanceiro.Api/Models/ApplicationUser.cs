using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFinanceiro.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(150)")]
        public required string FullName { get; set; }
        public ICollection<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();
        public ICollection<AccountPayable> AccountsPayable { get; set; } = new List<AccountPayable>();
        public ICollection<AccountReceivable> AccountsReceivable { get; set; } = new List<AccountReceivable>();
        public ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<RecurringAccountReceivable> RecurringAccountsReceivable { get; set; } = new List<RecurringAccountReceivable>();
    }
}