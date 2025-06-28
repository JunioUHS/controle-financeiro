using AutoMapper;
using ControleFinanceiro.Api.DTOs.CreditCard;
using ControleFinanceiro.Api.Models;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Results;
using ControleFinanceiro.Api.Services.Contracts;

namespace ControleFinanceiro.Api.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ICreditCardRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreditCardService(ICreditCardRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreditCardDto>> CreateAsync(string userId, CreditCardCreateDto dto)
        {
            var existing = await _repository.GetByNameAsync(dto.Name, userId);
            if (existing != null)
                return Result<CreditCardDto>.Failure("J� existe um cart�o com esse nome.");

            var card = _mapper.Map<CreditCard>(dto);
            card.UserId = userId;

            await _repository.AddAsync(card);
            var success = await _unitOfWork.SaveChangesAsync();

            if (success == 0)
                return Result<CreditCardDto>.Failure("Erro ao criar cart�o.");

            var resultDto = _mapper.Map<CreditCardDto>(card);
            return Result<CreditCardDto>.Success(resultDto);
        }

        public async Task<Result<IEnumerable<CreditCardDto>>> GetAllAsync(string userId)
        {
            var cards = await _repository.GetAllAsync(userId);
            var result = _mapper.Map<IEnumerable<CreditCardDto>>(cards);
            return Result<IEnumerable<CreditCardDto>>.Success(result);
        }

        public async Task<Result<CreditCardDto>> GetByIdAsync(int id, string userId)
        {
            var card = await _repository.GetByIdAsync(id, userId);
            if (card == null)
                return Result<CreditCardDto>.Failure("Cart�o n�o encontrado.");

            var dto = _mapper.Map<CreditCardDto>(card);
            return Result<CreditCardDto>.Success(dto);
        }

        public async Task<Result<CreditCardDto>> UpdateAsync(int id, string userId, CreditCardUpdateDto dto)
        {
            var card = await _repository.GetByIdAsync(id, userId);
            if (card == null)
                return Result<CreditCardDto>.Failure("Cart�o n�o encontrado.");

            _mapper.Map(dto, card);

            _repository.Update(card);
            var success = await _unitOfWork.SaveChangesAsync();

            if (success == 0)
                return Result<CreditCardDto>.Failure("Erro ao atualizar cart�o.");

            var resultDto = _mapper.Map<CreditCardDto>(card);
            return Result<CreditCardDto>.Success(resultDto);
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var card = await _repository.GetByIdAsync(id, userId);
            if (card == null)
                return Result.Failure("Cart�o n�o encontrado.");

            _repository.Delete(card);
            var success = await _unitOfWork.SaveChangesAsync();

            if (success == 0)
                return Result.Failure("Erro ao excluir cart�o.");

            return Result.Success();
        }
    }
}