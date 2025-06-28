using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface IRecurringAccountReceivableRepository
    {
        Task AddAsync(RecurringAccountReceivable recurring);
        Task<RecurringAccountReceivable?> GetByIdAsync(int id, string userId);
        Task<IEnumerable<RecurringAccountReceivable>> GetAllAsync(string userId);
        void Update(RecurringAccountReceivable recurring);
        void Delete(RecurringAccountReceivable recurring);
    }
}