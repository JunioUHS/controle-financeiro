using System.Threading.Tasks;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}