using AutoMapper;
using ControleFinanceiro.Api.DTOs.AccountPayable;
using ControleFinanceiro.Api.DTOs.AccountReceivable;
using ControleFinanceiro.Api.DTOs.Category;
using ControleFinanceiro.Api.DTOs.CreditCard;
using ControleFinanceiro.Api.DTOs.CreditCardPurchase;
using ControleFinanceiro.Api.DTOs.User;
using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<ApplicationUser, RegisterUserDto>().ReverseMap();
            CreateMap<ApplicationUser, CurrentUserDto>().ReverseMap();

            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

            // AccountPayable
            CreateMap<AccountPayable, AccountPayableDto>().ReverseMap();
            CreateMap<AccountPayableCreateDto, AccountPayable>();
            CreateMap<AccountPayableUpdateDto, AccountPayable>();

            // AccountReceivable
            CreateMap<AccountReceivable, AccountReceivableDto>().ReverseMap();
            CreateMap<AccountReceivableCreateDto, AccountReceivable>();
            CreateMap<AccountReceivableUpdateDto, AccountReceivable>();

            // RecurringAccountReceivable
            CreateMap<RecurringAccountReceivable, RecurringAccountReceivableDto>().ReverseMap();
            CreateMap<RecurringAccountReceivableCreateDto, RecurringAccountReceivable>();

            // CreditCard
            CreateMap<CreditCard, CreditCardDto>().ReverseMap();
            CreateMap<CreditCardCreateDto, CreditCard>();
            CreateMap<CreditCardUpdateDto, CreditCard>();

            // CreditCardPurchase
            CreateMap<CreditCardPurchase, CreditCardPurchaseDto>()
                .ForMember(dest => dest.Installments, opt => opt.MapFrom(src =>
                    src.Installments.OrderBy(i => i.NumberInstallment)))
                .ReverseMap();
            CreateMap<CreditCardPurchaseCreateDto, CreditCardPurchase>();

            // PurchaseInstallment
            CreateMap<PurchaseInstallment, PurchaseInstallmentDto>().ReverseMap();
        }
    }
}