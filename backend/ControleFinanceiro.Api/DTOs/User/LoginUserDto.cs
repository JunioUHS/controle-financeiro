using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.User
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "O nome de usu�rio � obrigat�rio.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha � obrigat�ria.")]
        public string Password { get; set; } = string.Empty;
    }
}