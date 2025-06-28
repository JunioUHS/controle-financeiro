using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface ICreditCardRepository
    {
        Task AddAsync(CreditCard card);
        Task<CreditCard?> GetByIdAsync(int id, string userId);
        Task<IEnumerable<CreditCard>> GetAllAsync(string userId);
        Task<CreditCard?> GetByNameAsync(string name, string userId);
        void Update(CreditCard card);
        void Delete(CreditCard card);
    }
}