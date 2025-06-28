using AutoMapper;
using ControleFinanceiro.Api.DTOs.AccountReceivable;
using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Results;
using ControleFinanceiro.Api.Services.Contracts;

namespace ControleFinanceiro.Api.Services
{
    public class RecurringAccountReceivableService : IRecurringAccountReceivableService
    {
        private readonly IRecurringAccountReceivableRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RecurringAccountReceivableService(
            IRecurringAccountReceivableRepository repository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RecurringAccountReceivableDto>> CreateAsync(string userId, RecurringAccountReceivableCreateDto dto)
        {
            if (dto.CategoryId == null)
                return Result<RecurringAccountReceivableDto>.Failure("Categoria � obrigat�ria.");

            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value, userId);
            if (category == null)
                return Result<RecurringAccountReceivableDto>.Failure("Categoria n�o encontrada.");

            var recurring = _mapper.Map<RecurringAccountReceivable>(dto);
            recurring.UserId = userId;
            recurring.Category = category;

            await _repository.AddAsync(recurring);
            var success = await _unitOfWork.SaveChangesAsync();

            if (success == 0)
                return Result<RecurringAccountReceivableDto>.Failure("Erro ao criar recorr�ncia.");

            var resultDto = _mapper.Map<RecurringAccountReceivableDto>(recurring);

            return Result<RecurringAccountReceivableDto>.Success(resultDto);
        }

        public async Task<Result<IEnumerable<RecurringAccountReceivableDto>>> GetAllAsync(string userId)
        {
            var recurrences = await _repository.GetAllAsync(userId);
            var result = _mapper.Map<IEnumerable<RecurringAccountReceivableDto>>(recurrences);
            return Result<IEnumerable<RecurringAccountReceivableDto>>.Success(result);
        }

        public async Task<Result<RecurringAccountReceivableDto>> GetByIdAsync(int id, string userId)
        {
            var recurrence = await _repository.GetByIdAsync(id, userId);
            if (recurrence == null)
                return Result<RecurringAccountReceivableDto>.Failure("Recorr�ncia n�o encontrada.");

            var dto = _mapper.Map<RecurringAccountReceivableDto>(recurrence);
            return Result<RecurringAccountReceivableDto>.Success(dto);
        }

        public async Task<Result> InactivateAsync(int id, string userId)
        {
            var recurrence = await _repository.GetByIdAsync(id, userId);
            if (recurrence == null)
                return Result.Failure("Recorr�ncia n�o encontrada.");

            recurrence.IsActive = false;
            _repository.Update(recurrence);

            var success = await _unitOfWork.SaveChangesAsync();
            if (success == 0)
                return Result.Failure("Erro ao inativar recorr�ncia.");

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var recurrence = await _repository.GetByIdAsync(id, userId);
            if (recurrence == null)
                return Result.Failure("Recorr�ncia n�o encontrada.");

            _repository.Delete(recurrence);

            var success = await _unitOfWork.SaveChangesAsync();
            if (success == 0)
                return Result.Failure("Erro ao excluir recorr�ncia.");

            return Result.Success();
        }
    }
}