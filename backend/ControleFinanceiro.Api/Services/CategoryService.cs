using AutoMapper;
using ControleFinanceiro.Api.DTOs.Category;
using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Results;
using ControleFinanceiro.Api.Services.Contracts;

namespace ControleFinanceiro.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryDto>> CreateAsync(string userId, CategoryCreateDto dto)
        {
            var existingCategory = await _repository.GetByNameAsync(dto.Name!, userId);
            if (existingCategory != null)
                return Result<CategoryDto>.Failure("J� existe uma categoria com esse nome.");

            var category = _mapper.Map<Category>(dto);

            category.UserId = userId;

            await _repository.AddAsync(category);
            var success = await _unitOfWork.SaveChangesAsync();

            if(success == 0)
                return Result<CategoryDto>.Failure("Erro ao criar a categoria. Verifique se as informa��es passadas est�o corretas");

            var resultDto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.Success(resultDto);
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(string userId)
        {
            var categories = await _repository.GetAllAsync(userId);
            var result = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Result<IEnumerable<CategoryDto>>.Success(result);
        }

        public async Task<Result<CategoryDto>> GetByIdAsync(int id, string userId)
        {
            var category = await _repository.GetByIdAsync(id, userId);
            if (category == null)
                return Result<CategoryDto>.Failure("Categoria n�o encontrada.");

            var dto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.Success(dto);
        }

        public async Task<Result<CategoryDto>> UpdateAsync(int id, string userId, CategoryUpdateDto dto)
        {
            var category = await _repository.GetByIdAsync(id, userId);
            if (category == null)
                return Result<CategoryDto>.Failure("Categoria n�o encontrada.");

            _mapper.Map(dto, category);
            _repository.Update(category);

            var success = await _unitOfWork.SaveChangesAsync();
            if (success == 0)
                return Result<CategoryDto>.Failure("Erro ao atualizar a categoria. Verifique se as informa��es passadas est�o corretas");

            var resultDto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.Success(resultDto);
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var category = await _repository.GetByIdAsync(id, userId);
            if (category == null)
                return Result.Failure("Categoria n�o encontrada.");

            _repository.Delete(category);

            var success = await _unitOfWork.SaveChangesAsync();
            if (success == 0)
                return Result.Failure("Erro ao excluir a categoria.");

            return Result.Success();
        }
    }
}