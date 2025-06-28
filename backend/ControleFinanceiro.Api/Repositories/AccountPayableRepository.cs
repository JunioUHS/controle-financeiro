using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Models.Context;
using ControleFinanceiro.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Repositories
{
    public class AccountPayableRepository : IAccountPayableRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountPayableRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AccountPayable accountPayable)
        {
            await _context.AccountsPayable.AddAsync(accountPayable);
        }

        public async Task<AccountPayable?> GetByIdAsync(int id, string userId)
        {
            return await _context.AccountsPayable
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<IEnumerable<AccountPayable>> GetAllAsync(string userId, DateTime? start = null, DateTime? end = null)
        {
            var query = _context.AccountsPayable
                .Include(p => p.Category)
                .Where(p => p.UserId == userId);

            if (start.HasValue)
                query = query.Where(p => p.DueDate >= start.Value);

            if (end.HasValue)
                query = query.Where(p => p.DueDate <= end.Value);

            query = query.OrderByDescending(p => p.DueDate);

            return await query.ToListAsync();
        }

        public void Update(AccountPayable accountPayable)
        {
            _context.AccountsPayable.Update(accountPayable);
        }

        public void Delete(AccountPayable accountPayable)
        {
            _context.AccountsPayable.Remove(accountPayable);
        }
    }
}