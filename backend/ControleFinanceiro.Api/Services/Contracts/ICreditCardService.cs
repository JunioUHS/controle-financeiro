using ControleFinanceiro.Api.DTOs.CreditCard;
using ControleFinanceiro.Api.Results;

namespace ControleFinanceiro.Api.Services.Contracts
{
    public interface ICreditCardService
    {
        Task<Result<CreditCardDto>> CreateAsync(string userId, CreditCardCreateDto dto);
        Task<Result<IEnumerable<CreditCardDto>>> GetAllAsync(string userId);
        Task<Result<CreditCardDto>> GetByIdAsync(int id, string userId);
        Task<Result<CreditCardDto>> UpdateAsync(int id, string userId, CreditCardUpdateDto dto);
        Task<Result> DeleteAsync(int id, string userId);
    }
}