using ControleFinanceiro.Api.DTOs.User;
using ControleFinanceiro.Api.Extensions;
using ControleFinanceiro.Api.Responses;
using ControleFinanceiro.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var result = await _userService.RegisterAsync(dto);

            if (!result.Succeeded)
                return BadRequest(ApiResponse.Fail(result.ToApiErrors()));

            return Ok(ApiResponse<object>.Ok("Cadastro feito com sucesso."));
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponse.Fail("Usuário não autenticado."));

            // Recupera o usuário do banco (pode criar um método no IUserService se preferir)
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound(ApiResponse.Fail("Usuário não encontrado."));

            return Ok(ApiResponse<CurrentUserDto>.Ok(user));
        }
    }
}
