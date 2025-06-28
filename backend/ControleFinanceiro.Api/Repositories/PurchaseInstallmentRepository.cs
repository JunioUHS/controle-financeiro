using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Models.Context;
using ControleFinanceiro.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Repositories
{
    public class PurchaseInstallmentRepository : IPurchaseInstallmentRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseInstallmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseInstallment?> GetByIdAsync(int id, string userId)
        {
            return await _context.PurchaseInstallments
                .Include(i => i.Purchase)
                .ThenInclude(p => p.CreditCard)
                .FirstOrDefaultAsync(i => i.Id == id && i.Purchase.CreditCard.UserId == userId);
        }

        public async Task<IEnumerable<PurchaseInstallment>> GetAllByPurchaseIdAsync(int purchaseId, string userId)
        {
            return await _context.PurchaseInstallments
                .Include(i => i.Purchase)
                .ThenInclude(p => p.CreditCard)
                .Where(i => i.PurchaseId == purchaseId && i.Purchase.CreditCard.UserId == userId)
                .ToListAsync();
        }

        public void Update(PurchaseInstallment installment)
        {
            _context.PurchaseInstallments.Update(installment);
        }
    }
}