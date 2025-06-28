using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Models.Context;
using ControleFinanceiro.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task<Category?> GetByIdAsync(int id, string userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<Category?> GetByNameAsync(string name, string userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower() && c.UserId == userId);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }
    }
}