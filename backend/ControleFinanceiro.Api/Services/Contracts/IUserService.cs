using ControleFinanceiro.Api.DTOs.User;
using ControleFinanceiro.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace ControleFinanceiro.Api.Services.Contracts
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<(string? token, string? refreshToken)> LoginAsync(LoginUserDto dto);
        Task<(string? token, string? refreshToken)> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task<CurrentUserDto?> GetByIdAsync(string userId);
    }
}