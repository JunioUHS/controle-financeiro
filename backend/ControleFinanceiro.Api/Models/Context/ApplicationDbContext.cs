using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ControleFinanceiro.Api.Repositories.Contracts;

namespace ControleFinanceiro.Api.Models.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<AccountPayable> AccountsPayable { get; set; } = null!;
        public DbSet<AccountReceivable> AccountsReceivable { get; set; } = null!;
        public DbSet<CreditCard> CreditCards { get; set; } = null!;
        public DbSet<CreditCardPurchase> CreditCardPurchases { get; set; } = null!;
        public DbSet<PurchaseInstallment> PurchaseInstallments { get; set; } = null!;
        public DbSet<RecurringAccountReceivable> RecurringAccountsReceivable { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRefreshToken>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasOne(t => t.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(t => t.UserId)
                      .IsRequired();
            });

            builder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Categories)
                      .HasForeignKey(c => c.UserId)
                      .IsRequired();
            });

            builder.Entity<AccountPayable>(entity =>
            {
                entity.HasKey(ap => ap.Id);
                entity.Property(ap => ap.Description).IsRequired();
                entity.Property(ap => ap.Value).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(ap => ap.DueDate).HasColumnType("date").IsRequired();
                entity.HasOne(ap => ap.Category)
                      .WithMany(c => c.AccountsPayable)
                      .HasForeignKey(ap => ap.CategoryId)
                      .IsRequired();
                entity.HasOne(ap => ap.User)
                      .WithMany(u => u.AccountsPayable)
                      .HasForeignKey(ap => ap.UserId)
                      .IsRequired();
            });

            builder.Entity<AccountReceivable>(entity =>
            {
                entity.HasKey(ar => ar.Id);
                entity.Property(ar => ar.Description).IsRequired();
                entity.Property(ar => ar.Value).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(ar => ar.ReceiptDate).HasColumnType("date").IsRequired();
                entity.HasOne(ar => ar.Category)
                      .WithMany(c => c.AccountsReceivable)
                      .HasForeignKey(ar => ar.CategoryId)
                      .IsRequired();
                entity.HasOne(ar => ar.User)
                      .WithMany(u => u.AccountsReceivable)
                      .HasForeignKey(ar => ar.UserId)
                      .IsRequired();
            });

            builder.Entity<CreditCard>(entity =>
            {
                entity.HasKey(cc => cc.Id);
                entity.Property(cc => cc.Name).HasMaxLength(100).IsRequired();
                entity.Property(cc => cc.Limit).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(cc => cc.CurrentBalance).HasColumnType("decimal(18,2)").IsRequired();
                entity.HasOne(cc => cc.User)
                      .WithMany(u => u.CreditCards)
                      .HasForeignKey(cc => cc.UserId)
                      .IsRequired();
            });

            builder.Entity<CreditCardPurchase>(entity =>
            {
                entity.HasKey(cp => cp.Id);
                entity.Property(cp => cp.Description).IsRequired();
                entity.Property(cp => cp.Value).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(cp => cp.PurchaseDate).HasColumnType("date").IsRequired();
                entity.HasOne(cp => cp.Category)
                      .WithMany(c => c.CreditCardPurchases)
                      .HasForeignKey(cp => cp.CategoryId)
                      .IsRequired();
                entity.HasOne(cp => cp.CreditCard)
                      .WithMany(cc => cc.Purchases)
                      .HasForeignKey(cp => cp.CreditCardId)
                      .IsRequired();
            });

            builder.Entity<PurchaseInstallment>(entity =>
            {
                entity.HasKey(pi => pi.Id);
                entity.Property(pi => pi.Value).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(pi => pi.DueDate).HasColumnType("date").IsRequired();
                entity.HasOne(pi => pi.Purchase)
                      .WithMany(cp => cp.Installments)
                      .HasForeignKey(pi => pi.PurchaseId)
                      .IsRequired();
            });

            builder.Entity<RecurringAccountReceivable>(entity =>
            {
                entity.HasKey(rar => rar.Id);
                entity.Property(rar => rar.Description).IsRequired();
                entity.Property(rar => rar.Value).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(rar => rar.StartDate).HasColumnType("date").IsRequired();
                entity.Property(rar => rar.EndDate).HasColumnType("date");
                entity.Property(rar => rar.Period).IsRequired().HasMaxLength(20);
                entity.HasOne(rar => rar.Category)
                      .WithMany(c => c.RecurringAccountsReceivable)
                      .HasForeignKey(rar => rar.CategoryId)
                      .IsRequired();
                entity.HasOne(rar => rar.User)
                      .WithMany(u => u.RecurringAccountsReceivable)
                      .HasForeignKey(rar => rar.UserId)
                      .IsRequired();
            });
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}