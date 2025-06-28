using ControleFinanceiro.Api.DTOs.Category;
using ControleFinanceiro.Api.Results;

namespace ControleFinanceiro.Api.Services.Contracts
{
    public interface ICategoryService
    {
        Task<Result<CategoryDto>> CreateAsync(string userId, CategoryCreateDto dto);
        Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(string userId);
        Task<Result<CategoryDto>> GetByIdAsync(int id, string userId);
        Task<Result<CategoryDto>> UpdateAsync(int id, string userId, CategoryUpdateDto dto);
        Task<Result> DeleteAsync(int id, string userId);
    }
}