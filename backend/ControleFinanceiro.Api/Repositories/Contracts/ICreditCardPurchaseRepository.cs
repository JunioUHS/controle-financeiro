using ControleFinanceiro.Api.DTOs.CreditCardPurchase;
using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface ICreditCardPurchaseRepository
    {
        Task AddAsync(CreditCardPurchase purchase);
        Task<CreditCardPurchase?> GetByIdAsync(int id, string userId);
        Task<IEnumerable<CreditCardPurchase>> GetAllAsync(string userId, CreditCardPurchaseFilterDto filter);
        void Update(CreditCardPurchase purchase);
        void Delete(CreditCardPurchase purchase);
        Task<PurchaseInstallment?> GetInstallmentByIdAsync(int installmentId, string userId);
    }
}