using ControleFinanceiro.Api.DTOs.CreditCardPurchase;
using ControleFinanceiro.Api.Results;

namespace ControleFinanceiro.Api.Services.Contracts
{
    public interface ICreditCardPurchaseService
    {
        Task<Result<CreditCardPurchaseDto>> CreateAsync(string userId, CreditCardPurchaseCreateDto dto);
        Task<Result<IEnumerable<CreditCardPurchaseDto>>> GetAllAsync(string userId, CreditCardPurchaseFilterDto filter);
        Task<Result<CreditCardPurchaseDto>> GetByIdAsync(int id, string userId);
        Task<Result> MarkInstallmentAsPaidAsync(int installmentId, string userId);
        Task<Result> DeleteAsync(int id, string userId);
    }
}