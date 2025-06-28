using ControleFinanceiro.Api.DTOs.AccountReceivable;
using ControleFinanceiro.Api.Results;

namespace ControleFinanceiro.Api.Services.Contracts
{
    public interface IRecurringAccountReceivableService
    {
        Task<Result<RecurringAccountReceivableDto>> CreateAsync(string userId, RecurringAccountReceivableCreateDto dto);
        Task<Result<IEnumerable<RecurringAccountReceivableDto>>> GetAllAsync(string userId);
        Task<Result<RecurringAccountReceivableDto>> GetByIdAsync(int id, string userId);
        Task<Result> InactivateAsync(int id, string userId);
        Task<Result> DeleteAsync(int id, string userId);
    }
}