using ControleFinanceiro.Api.DTOs.CreditCard;
using ControleFinanceiro.Api.Extensions;
using ControleFinanceiro.Api.Responses;
using ControleFinanceiro.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardService _service;

        public CreditCardController(ICreditCardService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreditCardCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.CreateAsync(userId!, dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<CreditCardDto>.Fail(result.Error!));

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, ApiResponse<CreditCardDto>.Ok(result.Value));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetAllAsync(userId!);
            return Ok(ApiResponse<IEnumerable<CreditCardDto>>.Ok(result.Value));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetByIdAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<CreditCardDto>.Fail(result.Error!));

            return Ok(ApiResponse<CreditCardDto>.Ok(result.Value));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreditCardUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.UpdateAsync(id, userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<CreditCardDto>.Fail(result.Error!));

            return Ok(ApiResponse<CreditCardDto>.Ok(result.Value));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.DeleteAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse.Ok("Cart�o deletado com sucesso."));
        }
    }
}