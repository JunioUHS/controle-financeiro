using ControleFinanceiro.Api.Models;
using System.Security.Claims;

namespace ControleFinanceiro.Api.Services.Contracts
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}