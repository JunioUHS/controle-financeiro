using ControleFinanceiro.Api.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Services.Contracts;
using ControleFinanceiro.Api.DTOs.User;

namespace ControleFinanceiro.Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUserRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IMapper mapper,
            IUserRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            var user = _mapper.Map<ApplicationUser>(dto);
            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<(string? token, string? refreshToken)> LoginAsync(LoginUserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return (null, null);

            var token = _jwtService.GenerateToken(user);

            var userRefreshToken = UserRefreshToken.Create(user);

            await _refreshTokenRepository.AddAsync(userRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return (token, userRefreshToken.Token);
        }

        public async Task<(string? token, string? refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var userRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (userRefreshToken == null || userRefreshToken.Expiration <= DateTime.UtcNow)
                return (null, null);

            _refreshTokenRepository.RevokeAsync(userRefreshToken);

            var user = userRefreshToken.User;
            var newToken = _jwtService.GenerateToken(user);

            var newUserRefreshToken = UserRefreshToken.Create(user);

            await _refreshTokenRepository.AddAsync(newUserRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return (newToken, newUserRefreshToken.Token);
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var userRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (userRefreshToken != null)
            {
                _refreshTokenRepository.RevokeAsync(userRefreshToken);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<CurrentUserDto?> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null ? _mapper.Map<CurrentUserDto>(user) : null;
        }
    }
}