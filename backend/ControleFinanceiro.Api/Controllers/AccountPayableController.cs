using ControleFinanceiro.Api.DTOs.AccountPayable;
using ControleFinanceiro.Api.Extensions;
using ControleFinanceiro.Api.Responses;
using ControleFinanceiro.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountPayableController : ControllerBase
    {
        private readonly IAccountPayableService _service;
        public AccountPayableController(IAccountPayableService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.GetAllAsync(userId!);

            return Ok(ApiResponse<IEnumerable<AccountPayableDto>>.Ok(result.Value));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.GetByIdAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<AccountPayableDto>.Fail(result.Error!));

            return Ok(ApiResponse<AccountPayableDto>.Ok(result.Value));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountPayableCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.CreateAsync(userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<AccountPayableDto>.Fail(result.Error!));

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, ApiResponse<AccountPayableDto>.Ok(result.Value));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountPayableUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.UpdateAsync(id, userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<AccountPayableDto>.Fail(result.Error!));

            return Ok(ApiResponse<AccountPayableDto>.Ok(result.Value));
        }

        [HttpPatch("{id}/MarkAsPaid")]
        public async Task<IActionResult> MaskAsPaid(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.MarkAsPaidAsync(id, userId!);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<AccountPayableDto>.Fail(result.Error!));

            return Ok(ApiResponse<AccountPayableDto>.Ok(result.Value));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.DeleteAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse.Ok());
        }
    }
}
