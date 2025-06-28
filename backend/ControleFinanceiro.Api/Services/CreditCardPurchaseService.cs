using AutoMapper;
using ControleFinanceiro.Api.DTOs.AccountReceivable;
using ControleFinanceiro.Api.DTOs.CreditCardPurchase;
using ControleFinanceiro.Api.Factories;
using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Results;
using ControleFinanceiro.Api.Services.Contracts;

namespace ControleFinanceiro.Api.Services
{
    public class CreditCardPurchaseService : ICreditCardPurchaseService
    {
        private readonly ICreditCardPurchaseRepository _repository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPurchaseInstallmentRepository _purchaseInstallmentRepository;

        public CreditCardPurchaseService(
            ICreditCardPurchaseRepository repository,
            ICreditCardRepository creditCardRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IPurchaseInstallmentRepository purchaseInstallmentRepository)
        {
            _repository = repository;
            _creditCardRepository = creditCardRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _purchaseInstallmentRepository = purchaseInstallmentRepository;
        }

        private async Task<Result<Category>> ValidateCategoryAsync(int? categoryId, string userId)
        {
            if (categoryId == null)
                return Result<Category>.Failure("Categoria � obrigat�ria.");

            var category = await _categoryRepository.GetByIdAsync(categoryId.Value, userId);
            if (category == null)
                return Result<Category>.Failure("Categoria n�o encontrada.");

            return Result<Category>.Success(category);
        }

        private async Task<Result<CreditCard>> ValidateCreditCardAsync(int? creditCardId, string userId)
        {
            if (creditCardId == null)
                return Result<CreditCard>.Failure("Cart�o de cr�dito � obrigat�rio.");

            var creditCard = await _creditCardRepository.GetByIdAsync(creditCardId.Value, userId);
            if (creditCard == null)
                return Result<CreditCard>.Failure("Cart�o n�o encontrado.");

            return Result<CreditCard>.Success(creditCard);
        }

        private async Task<Result<(Category category, CreditCard creditCard)>> Validate(int? categoryId, int? creditCardId, decimal value, string userId)
        {
            // Valida se a categoria informada existe
            var categoryResult = await ValidateCategoryAsync(categoryId, userId);
            if (!categoryResult.IsSuccess)
                return Result<(Category, CreditCard)>.Failure(categoryResult.Error!);

            // Valida se o cart�o de cr�dito informado existe
            var creditCardResult = await ValidateCreditCardAsync(creditCardId, userId);
            if (!creditCardResult.IsSuccess)
                return Result<(Category, CreditCard)>.Failure(creditCardResult.Error!);

            // Valida se o valor da compra n�o excede o limite do cart�o
            var creditCard = creditCardResult.Value;
            if (creditCard.CurrentBalance + value > creditCard.Limit)
                return Result<(Category, CreditCard)>.Failure("Limite do cart�o excedido.");

            return Result<(Category, CreditCard)>.Success((categoryResult.Value, creditCard));
        }

        public async Task<Result<CreditCardPurchaseDto>> CreateAsync(string userId, CreditCardPurchaseCreateDto dto)
        {
            var validateResult = await Validate(dto.CategoryId, dto.CreditCardId, dto.Value, userId);
            if (!validateResult.IsSuccess)
                return Result<CreditCardPurchaseDto>.Failure(validateResult.Error!);

            var (category, creditCard) = validateResult.Value;

            var purchase = CreditCardPurchaseFactory.Create(dto, category, creditCard, userId);

            // Atualiza o saldo utilizado atual do cart�o de cr�dito
            creditCard.CurrentBalance += dto.Value;

            await _repository.AddAsync(purchase);
            var success = await _unitOfWork.SaveChangesAsync();

            if (success == 0)
                return Result<CreditCardPurchaseDto>.Failure("Erro ao registrar compra.");

            var resultDto = _mapper.Map<CreditCardPurchaseDto>(purchase);
            return Result<CreditCardPurchaseDto>.Success(resultDto);
        }

        public async Task<Result<IEnumerable<CreditCardPurchaseDto>>> GetAllAsync(string userId, CreditCardPurchaseFilterDto filter)
        {
            var purchases = await _repository.GetAllAsync(userId, filter);
            var result = _mapper.Map<IEnumerable<CreditCardPurchaseDto>>(purchases);
            return Result<IEnumerable<CreditCardPurchaseDto>>.Success(result);
        }

        public async Task<Result<CreditCardPurchaseDto>> GetByIdAsync(int id, string userId)
        {
            var purchase = await _repository.GetByIdAsync(id, userId);
            if (purchase == null)
                return Result<CreditCardPurchaseDto>.Failure("Compra n�o encontrada.");

            var dto = _mapper.Map<CreditCardPurchaseDto>(purchase);
            return Result<CreditCardPurchaseDto>.Success(dto);
        }

        public async Task<Result> MarkInstallmentAsPaidAsync(int installmentId, string userId)
        {
            var purchaseInstallment = await _purchaseInstallmentRepository.GetByIdAsync(installmentId, userId);

            if (purchaseInstallment == null)
                return Result.Failure("Parcela n�o encontrada.");

            if (purchaseInstallment.IsPaid)
                return Result.Failure("Parcela j� est� paga.");

            purchaseInstallment.IsPaid = true;
            _purchaseInstallmentRepository.Update(purchaseInstallment);

            var success = await _unitOfWork.SaveChangesAsync();
            if (success == 0)
                return Result.Failure("Erro ao marcar parcela como paga.");
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var purchase = await _repository.GetByIdAsync(id, userId);
            if (purchase == null)
                return Result.Failure("Compra n�o encontrada.");

            _repository.Delete(purchase);
            var success = await _unitOfWork.SaveChangesAsync();

            if (success == 0)
                return Result.Failure("Erro ao excluir compra.");

            return Result.Success();
        }
    }
}