using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface IAccountPayableRepository
    {
        Task AddAsync(AccountPayable accountPayable);
        Task<AccountPayable?> GetByIdAsync(int id, string userId);
        Task<IEnumerable<AccountPayable>> GetAllAsync(string userId, DateTime? start = null, DateTime? end = null);
        void Update(AccountPayable accountPayable);
        void Delete(AccountPayable accountPayable);
    }
}
