using AutoMapper;
using ControleFinanceiro.Api.DTOs.AccountPayable;
using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Results;
using ControleFinanceiro.Api.Services.Contracts;

namespace ControleFinanceiro.Api.Services
{
    public class AccountPayableService : IAccountPayableService
    {
        private readonly IAccountPayableRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountPayableService(
            IAccountPayableRepository repository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        private async Task<Result> ValidateCategoryAsync(int? categoryId, string userId)
        {
            if (categoryId == null)
                return Result.Failure("Categoria é obrigatória.");

            var category = await _categoryRepository.GetByIdAsync(categoryId.Value, userId);
            if (category == null)
                return Result.Failure("Categoria não encontrada.");

            return Result.Success();
        }

        public async Task<Result<AccountPayableDto>> CreateAsync(string userId, AccountPayableCreateDto dto)
        {
            var categoryValidation = await ValidateCategoryAsync(dto.CategoryId, userId);
            if (!categoryValidation.IsSuccess)
                return Result<AccountPayableDto>.Failure(categoryValidation.Error!);

            var accountPayable = _mapper.Map<AccountPayable>(dto);
            accountPayable.UserId = userId;

            await _repository.AddAsync(accountPayable);
            var success = await _unitOfWork.SaveChangesAsync();

            if (success == 0)
                return Result<AccountPayableDto>.Failure("Erro ao criar conta a pagar. Verifique se as informações passadas estão corretas.");

            var resultDto = _mapper.Map<AccountPayableDto>(accountPayable);

            return Result<AccountPayableDto>.Success(resultDto);
        }

        public async Task<Result<AccountPayableDto>> GetByIdAsync(int id, string userId)
        {
            var accountPayable = await _repository.GetByIdAsync(id, userId);

            if (accountPayable == null)
                return Result<AccountPayableDto>.Failure("Conta a pagar não encontrada.");

            var dto = _mapper.Map<AccountPayableDto>(accountPayable);
            return Result<AccountPayableDto>.Success(dto);
        }

        public async Task<Result<IEnumerable<AccountPayableDto>>> GetAllAsync(string userId)
        {
            var accounts = await _repository.GetAllAsync(userId);
            var result = _mapper.Map<IEnumerable<AccountPayableDto>>(accounts);
            return Result<IEnumerable<AccountPayableDto>>.Success(result);
        }

        public async Task<Result<AccountPayableDto>> UpdateAsync(int id, string userId, AccountPayableUpdateDto dto)
        {
            var accountPayable = await _repository.GetByIdAsync(id, userId);

            if (accountPayable == null)
                return Result<AccountPayableDto>.Failure("Conta a pagar não encontrada.");

            var categoryValidation = await ValidateCategoryAsync(dto.CategoryId, userId);
            if (!categoryValidation.IsSuccess)
                return Result<AccountPayableDto>.Failure(categoryValidation.Error!);

            var updatedAccountPayable = _mapper.Map(dto, accountPayable);

            _repository.Update(updatedAccountPayable);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<AccountPayableDto>(updatedAccountPayable);

            return Result<AccountPayableDto>.Success(resultDto);
        }

        public async Task<Result<AccountPayableDto>> MarkAsPaidAsync(int id, string userId)
        {
            var accountPayable = await _repository.GetByIdAsync(id, userId);

            if (accountPayable == null)
                return Result<AccountPayableDto>.Failure("Conta a pagar não encontrada.");

            accountPayable.IsPaid = true;

            _repository.Update(accountPayable);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<AccountPayableDto>(accountPayable);

            return Result<AccountPayableDto>.Success(resultDto);
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var accountPayable = await _repository.GetByIdAsync(id, userId);

            if (accountPayable == null)
                return Result.Failure("Conta a pagar não encontrada.");

            _repository.Delete(accountPayable);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
