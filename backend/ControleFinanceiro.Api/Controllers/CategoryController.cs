using ControleFinanceiro.Api.DTOs.Category;
using ControleFinanceiro.Api.Extensions;
using ControleFinanceiro.Api.Responses;
using ControleFinanceiro.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.GetAllAsync(userId!);
            return Ok(ApiResponse<IEnumerable<CategoryDto>>.Ok(result.Value));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.GetByIdAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<CategoryDto>.Fail(result.Error!));

            return Ok(ApiResponse<CategoryDto>.Ok(result.Value));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.CreateAsync(userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<CategoryDto>.Fail(result.Error!));

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, ApiResponse<CategoryDto>.Ok(result.Value));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail(ModelState.ToApiErrors()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.UpdateAsync(id, userId!, dto);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<CategoryDto>.Fail(result.Error!));

            return Ok(ApiResponse<CategoryDto>.Ok(result.Value));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.DeleteAsync(id, userId!);
            if (!result.IsSuccess)
                return NotFound(ApiResponse.Fail(result.Error!));

            return Ok(ApiResponse.Ok("Categoria deletada com sucesso."));
        }
    }
}