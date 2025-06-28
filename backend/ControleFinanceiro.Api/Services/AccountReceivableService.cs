using AutoMapper;
using ControleFinanceiro.Api.DTOs.AccountReceivable;
using ControleFinanceiro.Api.DTOs.Category;
using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Results;
using ControleFinanceiro.Api.Services.Contracts;

namespace ControleFinanceiro.Api.Services
{
    public class AccountReceivableService : IAccountReceivableService
    {
        private readonly IAccountReceivableRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecurringAccountReceivableRepository _recurringRepository;

        public AccountReceivableService(
            IAccountReceivableRepository repository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IRecurringAccountReceivableRepository recurringRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _recurringRepository = recurringRepository;
        }

        private async Task<Result> ValidateCategoryAsync(int? categoryId, string userId)
        {
            if (categoryId == null)
                return Result.Failure("Categoria � obrigat�ria.");

            var category = await _categoryRepository.GetByIdAsync(categoryId.Value, userId);
            if (category == null)
                return Result.Failure("Categoria n�o encontrada.");

            return Result.Success();
        }

        public async Task<Result<AccountReceivableDto>> CreateAsync(string userId, AccountReceivableCreateDto dto)
        {
            var categoryValidation = await ValidateCategoryAsync(dto.CategoryId, userId);
            if (!categoryValidation.IsSuccess)
                return Result<AccountReceivableDto>.Failure(categoryValidation.Error!);

            var accountReceivable = _mapper.Map<AccountReceivable>(dto);
            accountReceivable.UserId = userId;

            await _repository.AddAsync(accountReceivable);
            var success = await _unitOfWork.SaveChangesAsync();

            if (success == 0)
                return Result<AccountReceivableDto>.Failure("Erro ao criar conta a receber. Verifique se as informa��es passadas est�o corretas");

            var resultDto = _mapper.Map<AccountReceivableDto>(accountReceivable);

            return Result<AccountReceivableDto>.Success(resultDto);
        }

        public async Task<Result<AccountReceivableDto>> GetByIdAsync(int id, string userId)
        {
            var accountReceivable = await _repository.GetByIdAsync(id, userId);

            if (accountReceivable == null)
                return Result<AccountReceivableDto>.Failure("Conta a receber n�o encontrada.");

            var dto = _mapper.Map<AccountReceivableDto>(accountReceivable);
            return Result<AccountReceivableDto>.Success(dto);
        }

        public async Task<Result<IEnumerable<AccountReceivableDto>>> GetAllAsync(string userId)
        {
            await GenerateRecurringEntriesAsync(userId, DateTime.Today.AddMonths(1));

            var accounts = await _repository.GetAllAsync(userId);
            var result = _mapper.Map<IEnumerable<AccountReceivableDto>>(accounts);
            return Result<IEnumerable<AccountReceivableDto>>.Success(result);
        }

        public async Task<Result<AccountReceivableDto>> UpdateAsync(int id, string userId, AccountReceivableUpdateDto dto)
        {
            var accountReceivable = await _repository.GetByIdAsync(id, userId);

            if (accountReceivable == null)
                return Result<AccountReceivableDto>.Failure("Conta a receber n�o encontrada.");

            var categoryValidation = await ValidateCategoryAsync(dto.CategoryId, userId);
            if (!categoryValidation.IsSuccess)
                return Result<AccountReceivableDto>.Failure(categoryValidation.Error!);

            _mapper.Map(dto, accountReceivable);
            _repository.Update(accountReceivable);

            var success = await _unitOfWork.SaveChangesAsync();
            if (success == 0)
                return Result<AccountReceivableDto>.Failure("Erro ao atualizar conta a receber. Verifique se as informa��es passadas est�o corretas.");

            var resultDto = _mapper.Map<AccountReceivableDto>(accountReceivable);

            return Result<AccountReceivableDto>.Success(resultDto);
        }

        public async Task<Result> MarkAsReceivedAsync(int id, string userId)
        {
            var accountReceivable = await _repository.GetByIdAsync(id, userId);
            if (accountReceivable == null)
                return Result.Failure("Conta a receber n�o encontrada.");

            if (accountReceivable.IsReceived)
                return Result.Failure("Conta a receber j� est� marcada como paga.");

            accountReceivable.IsReceived = true;
            _repository.Update(accountReceivable);

            var success = await _unitOfWork.SaveChangesAsync();
            if (success == 0)
                return Result.Failure("Erro ao marcar conta a receber como paga. Verifique se as informa��es passadas est�o corretas.");

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var accountReceivable = await _repository.GetByIdAsync(id, userId);
            if (accountReceivable == null)
                return Result.Failure("Conta a receber n�o encontrada.");

            _repository.Delete(accountReceivable);

            var success = await _unitOfWork.SaveChangesAsync();
            if (success == 0)
                return Result.Failure("Erro ao deletar conta a receber. Verifique se as informa��es passadas est�o corretas.");

            return Result.Success();
        }

        public async Task<Result> GenerateRecurringEntriesAsync(string userId, DateTime limitDate)
        {
            // 1. Buscar todas as recorr�ncias ativas do usu�rio
            var recurrences = await _recurringRepository.GetAllAsync(userId);

            foreach (var recurrence in recurrences.Where(r => r.IsActive))
            {
                // 2. Calcular as datas previstas at� o limite
                var dates = CalculateNextDates(recurrence, limitDate);

                foreach (var date in dates)
                {
                    // 3. Verificar se j� existe lan�amento para essa data
                    var existing = await _repository.GetAllAsync(userId);
                    if (existing.Any(a =>
                        a.RecurringAccountReceivableId == recurrence.Id &&
                        a.ReceiptDate.Date == date.Date))
                    {
                        continue;
                    }

                    // 4. Criar novo lan�amento
                    var newEntry = new AccountReceivable
                    {
                        Description = recurrence.Description,
                        Value = recurrence.Value,
                        ReceiptDate = date,
                        IsReceived = false,
                        CategoryId = recurrence.CategoryId,
                        Category = recurrence.Category,
                        UserId = userId,
                        User = recurrence.User,
                        RecurringAccountReceivableId = recurrence.Id
                    };

                    await _repository.AddAsync(newEntry);
                }
            }

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
                return Result.Failure("Erro ao gerar lan�amentos recorrentes.");

            return Result.Success();
        }

        private IEnumerable<DateTime> CalculateNextDates(RecurringAccountReceivable recurrence, DateTime limitDate)
        {
            var dates = new List<DateTime>();
            var date = recurrence.StartDate.Date;

            if (recurrence.EndDate.HasValue)
                limitDate = recurrence.EndDate.Value;

            while (date <= limitDate)
            {
                dates.Add(date);

                date = recurrence.Period switch
                {
                    "Monthly" => date.AddMonths(1),
                    "Weekly" => date.AddDays(7),
                    "Daily" => date.AddDays(1),
                    _ => throw new InvalidOperationException("Invalid recurrence period.")
                };
            }

            return dates;
        }
    }
}