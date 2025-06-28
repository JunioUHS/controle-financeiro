using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Models.Context;
using ControleFinanceiro.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ApplicationDbContext _context;

        public CreditCardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreditCard card)
        {
            await _context.CreditCards.AddAsync(card);
        }

        public async Task<CreditCard?> GetByIdAsync(int id, string userId)
        {
            return await _context.CreditCards
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<IEnumerable<CreditCard>> GetAllAsync(string userId)
        {
            return await _context.CreditCards
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<CreditCard?> GetByNameAsync(string name, string userId)
        {
            return await _context.CreditCards
                .FirstOrDefaultAsync(c => c.Name == name && c.UserId == userId);
        }

        public void Update(CreditCard card)
        {
            _context.CreditCards.Update(card);
        }

        public void Delete(CreditCard card)
        {
            _context.CreditCards.Remove(card);
        }
    }
}