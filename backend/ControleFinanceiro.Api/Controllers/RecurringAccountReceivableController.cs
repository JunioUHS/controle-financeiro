using ControleFinanceiro.Api.DTOs.AccountReceivable;
using ControleFinanceiro.Api.Extensions;
using ControleFinanceiro.Api.Responses;
using ControleFinanceiro.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringAccountReceivableController : ControllerBase
    {
        private readonly IRecurringAccountReceivableService _service;

        public RecurringAccountReceivableController(IRecurringAccountReceivableService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecurringAccountReceivableCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.CreateAsync(userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse.Fail(result.Error!));

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, ApiResponse<RecurringAccountReceivableDto>.Ok(result.Value));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetAllAsync(userId!);
            return Ok(ApiResponse<IEnumerable<RecurringAccountReceivableDto>>.Ok(result.Value));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetByIdAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));
            return Ok(ApiResponse<RecurringAccountReceivableDto>.Ok(result.Value));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.DeleteAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse.Ok("Receita recorrente exclu�da com sucesso."));
        }

        [HttpPatch("{id}/Inactivate")]
        public async Task<IActionResult> Inactivate(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.InactivateAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse.Ok("Receita recorrente inativada com sucesso."));
        }
    }
}