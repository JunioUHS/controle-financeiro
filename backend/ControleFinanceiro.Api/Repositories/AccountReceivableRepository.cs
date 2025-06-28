using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Models.Context;
using ControleFinanceiro.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Repositories
{
    public class AccountReceivableRepository : IAccountReceivableRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountReceivableRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AccountReceivable accountReceivable)
        {
            await _context.AccountsReceivable.AddAsync(accountReceivable);
        }

        public async Task<AccountReceivable?> GetByIdAsync(int id, string userId)
        {
            return await _context.AccountsReceivable
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<IEnumerable<AccountReceivable>> GetAllAsync(string userId)
        {
            return await _context.AccountsReceivable
                .Include(a => a.Category)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ReceiptDate)
                .ToListAsync();
        }

        public void Update(AccountReceivable accountReceivable)
        {
            _context.AccountsReceivable.Update(accountReceivable);
        }

        public void Delete(AccountReceivable accountReceivable)
        {
            _context.AccountsReceivable.Remove(accountReceivable);
        }
    }
}