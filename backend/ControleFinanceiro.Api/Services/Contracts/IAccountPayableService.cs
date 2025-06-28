using ControleFinanceiro.Api.DTOs.AccountPayable;
using ControleFinanceiro.Api.Results;

namespace ControleFinanceiro.Api.Services.Contracts
{
    public interface IAccountPayableService
    {
        Task<Result<AccountPayableDto>> CreateAsync(string userId, AccountPayableCreateDto dto);
        Task<Result<AccountPayableDto>> GetByIdAsync(int id, string userId);
        Task<Result<IEnumerable<AccountPayableDto>>> GetAllAsync(string userId);
        Task<Result<AccountPayableDto>> UpdateAsync(int id, string userId, AccountPayableUpdateDto dto);
        Task<Result<AccountPayableDto>> MarkAsPaidAsync(int id, string userId);
        Task<Result> DeleteAsync(int id, string userId);
    }
}
