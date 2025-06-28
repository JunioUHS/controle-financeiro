using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Models.Context;
using ControleFinanceiro.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Api.Repositories
{
    public class UserRefreshTokenRepository : IUserRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserRefreshToken refreshToken)
        {
            await _context.UserRefreshTokens.AddAsync(refreshToken);
        }

        public async Task<UserRefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.UserRefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);
        }

        public void RevokeAsync(UserRefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
           _context.UserRefreshTokens.Update(refreshToken);
        }
    }
}