using ControleFinanceiro.Api.DTOs.AccountReceivable;
using ControleFinanceiro.Api.Results;

namespace ControleFinanceiro.Api.Services.Contracts
{
    public interface IAccountReceivableService
    {
        Task<Result<AccountReceivableDto>> CreateAsync(string userId, AccountReceivableCreateDto dto);
        Task<Result<AccountReceivableDto>> GetByIdAsync(int id, string userId);
        Task<Result<IEnumerable<AccountReceivableDto>>> GetAllAsync(string userId);
        Task<Result<AccountReceivableDto>> UpdateAsync(int id, string userId, AccountReceivableUpdateDto dto);
        Task<Result> MarkAsReceivedAsync(int id, string userId);
        Task<Result> DeleteAsync(int id, string userId);
        Task<Result> GenerateRecurringEntriesAsync(string userId, DateTime limitDate);
    }
}