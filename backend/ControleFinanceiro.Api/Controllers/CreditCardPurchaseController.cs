using ControleFinanceiro.Api.DTOs.CreditCardPurchase;
using ControleFinanceiro.Api.Extensions;
using ControleFinanceiro.Api.Responses;
using ControleFinanceiro.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardPurchaseController : ControllerBase
    {
        private readonly ICreditCardPurchaseService _service;

        public CreditCardPurchaseController(ICreditCardPurchaseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreditCardPurchaseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.CreateAsync(userId!, dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<CreditCardPurchaseDto>.Fail(result.Error!));

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, ApiResponse<CreditCardPurchaseDto>.Ok(result.Value));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CreditCardPurchaseFilterDto filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetAllAsync(userId!, filter);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse<IEnumerable<CreditCardPurchaseDto>>.Ok(result.Value));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetByIdAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<CreditCardPurchaseDto>.Fail(result.Error!));

            return Ok(ApiResponse<CreditCardPurchaseDto>.Ok(result.Value));
        }

        [HttpPatch("Installment/{installmentId}/MarkAsPaid")]
        public async Task<IActionResult> MarkInstallmentAsPaid(int installmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.MarkInstallmentAsPaidAsync(installmentId, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));
            return Ok(ApiResponse.Ok("Parcela marcada como paga com sucesso."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.DeleteAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse.Ok("Compra exclu�da com sucesso."));
        }
    }
}