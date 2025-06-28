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
    public class AccountReceivableController : ControllerBase
    {
        private readonly IAccountReceivableService _service;

        public AccountReceivableController(IAccountReceivableService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.GetAllAsync(userId!);
            return Ok(ApiResponse<IEnumerable<AccountReceivableDto>>.Ok(result.Value));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.GetByIdAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<AccountReceivableDto>.Fail(result.Error!));

            return Ok(ApiResponse<AccountReceivableDto>.Ok(result.Value));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountReceivableCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.CreateAsync(userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<AccountReceivableDto>.Fail(result.Error!));

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, ApiResponse<AccountReceivableDto>.Ok(result.Value));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountReceivableUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.UpdateAsync(id, userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<AccountReceivableDto>.Fail(result.Error!));

            return Ok(ApiResponse<AccountReceivableDto>.Ok(result.Value));
        }

        [HttpPatch("{id}/MarkAsReceived")]
        public async Task<IActionResult> MarkAsReceived(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.MarkAsReceivedAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));
            return Ok(ApiResponse.Ok("Conta a receber marcada como paga com sucesso."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.DeleteAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse.Ok("Conta a receber deletada com sucesso."));
        }

        [HttpPost("generate-recurring")]
        public async Task<IActionResult> GenerateRecurring([FromQuery] DateTime? limitDate = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dateLimit = limitDate ?? DateTime.Today.AddMonths(12);

            var result = await _service.GenerateRecurringEntriesAsync(userId!, dateLimit);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse.Ok("Recorrencia gerada com sucesso."));
        }
    }
}