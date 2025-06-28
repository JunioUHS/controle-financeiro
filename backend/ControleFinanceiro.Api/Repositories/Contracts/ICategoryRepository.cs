using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<Category?> GetByIdAsync(int id, string userId);
        Task<Category?> GetByNameAsync(string name, string userId);
        Task<IEnumerable<Category>> GetAllAsync(string userId);
        void Update(Category category);
        void Delete(Category category);
    }
}