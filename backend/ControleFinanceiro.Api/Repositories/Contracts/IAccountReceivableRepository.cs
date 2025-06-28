using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface IAccountReceivableRepository
    {
        Task AddAsync(AccountReceivable accountReceivable);
        Task<AccountReceivable?> GetByIdAsync(int id, string userId);
        Task<IEnumerable<AccountReceivable>> GetAllAsync(string userId);
        void Update(AccountReceivable accountReceivable);
        void Delete(AccountReceivable accountReceivable);
    }
}