using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Repositories.Contracts
{
    public interface IUserRefreshTokenRepository
    {
        Task AddAsync(UserRefreshToken refreshToken);
        Task<UserRefreshToken?> GetByTokenAsync(string token);
        void RevokeAsync(UserRefreshToken refreshToken);
    }
}