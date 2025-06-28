using System.Security.Cryptography;

namespace ControleFinanceiro.Api.Models
{
    public class UserRefreshToken
    {
        public const int DefaultExpirationDays = 7;

        public int Id { get; set; }
        public required string Token { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsRevoked { get; set; }
        public required string UserId { get; set; }  // FK para ApplicationUser
        public required ApplicationUser User { get; set; }

        // Factory para criar um novo refresh token já preenchido
        public static UserRefreshToken Create(ApplicationUser user)
        {
            return new UserRefreshToken
            {
                Token = GenerateRefreshToken(),
                Expiration = DateTime.UtcNow.AddDays(DefaultExpirationDays),
                IsRevoked = false,
                UserId = user.Id,
                User = user
            };
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}