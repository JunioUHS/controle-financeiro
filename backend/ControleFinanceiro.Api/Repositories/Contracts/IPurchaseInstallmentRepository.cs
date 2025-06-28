using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface IPurchaseInstallmentRepository
    {
        Task<PurchaseInstallment?> GetByIdAsync(int id, string userId);
        Task<IEnumerable<PurchaseInstallment>> GetAllByPurchaseIdAsync(int purchaseId, string userId);
        void Update(PurchaseInstallment installment);
    }
}