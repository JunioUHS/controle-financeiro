using ControleFinanceiro.Api.DTOs.CreditCardPurchase;
using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Models.Context;
using ControleFinanceiro.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Repositories
{
    public class CreditCardPurchaseRepository : ICreditCardPurchaseRepository
    {
        private readonly ApplicationDbContext _context;

        public CreditCardPurchaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreditCardPurchase purchase)
        {
            await _context.CreditCardPurchases.AddAsync(purchase);
        }

        public async Task<CreditCardPurchase?> GetByIdAsync(int id, string userId)
        {
            return await _context.CreditCardPurchases
                .Include(p => p.Category)
                .Include(p => p.CreditCard)
                .Include(p => p.Installments)
                .FirstOrDefaultAsync(p => p.Id == id && p.CreditCard.UserId == userId);
        }

        public async Task<IEnumerable<CreditCardPurchase>> GetAllAsync(string userId, CreditCardPurchaseFilterDto filter)
        {
            var query = _context.CreditCardPurchases
                .Where(p => p.CreditCard.UserId == userId);

            if (filter.CreditCardId.HasValue)
                query = query.Where(p => p.CreditCardId == filter.CreditCardId.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            var items = await query
                .Include(p => p.Category)
                .Include(p => p.CreditCard)
                .Include(p => p.Installments)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();

            return items;
        }

        public async Task<PurchaseInstallment?> GetInstallmentByIdAsync(int installmentId, string userId)
        {
            return await _context.PurchaseInstallments
                .Include(i => i.Purchase)
                .ThenInclude(p => p.CreditCard)
                .FirstOrDefaultAsync(i => i.Id == installmentId && i.Purchase.CreditCard.UserId == userId);
        }

        public void Update(CreditCardPurchase purchase)
        {
            _context.CreditCardPurchases.Update(purchase);
        }

        public void Delete(CreditCardPurchase purchase)
        {
            _context.CreditCardPurchases.Remove(purchase);
        }
    }
}