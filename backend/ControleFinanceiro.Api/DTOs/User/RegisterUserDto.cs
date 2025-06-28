using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.User
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "O nome de usu�rio � obrigat�rio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome de usu�rio deve ter entre 3 e 50 caracteres.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage ="O nome completo � obrigat�rio.")]
        [StringLength(150, ErrorMessage = "O nome completo deve at� 150 caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha � obrigat�ria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirma��o de senha � obrigat�ria.")]
        [Compare("Password", ErrorMessage = "A confirma��o de senha n�o corresponde � senha.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}