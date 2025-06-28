using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Models.Context;
using ControleFinanceiro.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Repositories
{
    public class RecurringAccountReceivableRepository : IRecurringAccountReceivableRepository
    {
        private readonly ApplicationDbContext _context;

        public RecurringAccountReceivableRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RecurringAccountReceivable recurring)
        {
            await _context.Set<RecurringAccountReceivable>().AddAsync(recurring);
        }

        public async Task<RecurringAccountReceivable?> GetByIdAsync(int id, string userId)
        {
            return await _context.Set<RecurringAccountReceivable>()
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
        }

        public async Task<IEnumerable<RecurringAccountReceivable>> GetAllAsync(string userId)
        {
            return await _context.Set<RecurringAccountReceivable>()
                .Include(r => r.Category)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public void Update(RecurringAccountReceivable recurring)
        {
            _context.Set<RecurringAccountReceivable>().Update(recurring);
        }

        public void Delete(RecurringAccountReceivable recurring)
        {
            _context.Set<RecurringAccountReceivable>().Remove(recurring);
        }
    }
}