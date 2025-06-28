using ControleFinanceiro.Api.DTOs.CreditCardPurchase;
using ControleFinanceiro.Api.Models;

namespace ControleFinanceiro.Api.Factories
{
    public class CreditCardPurchaseFactory
    {
        public static CreditCardPurchase Create(
            CreditCardPurchaseCreateDto dto,
            Category category,
            CreditCard creditCard,
            string userId)
        {

            var purchase = new CreditCardPurchase
            {
                Description = dto.Description!,
                Value = dto.Value,
                NumberInstallments = dto.NumberInstallments,
                PurchaseDate = dto.PurchaseDate,
                CategoryId = category.Id,
                Category = category,
                CreditCardId = creditCard.Id,
                CreditCard = creditCard,
            };

            var installments = GenerateInstallments(purchase);
            purchase.Installments = installments;

            return purchase;
        }

        private static List<PurchaseInstallment> GenerateInstallments(CreditCardPurchase purchase)
        {
            var valueInstallment = Math.Round(purchase.Value / purchase.NumberInstallments, 2);
            var installments = new List<PurchaseInstallment>();

            for (int i = 0; i < purchase.NumberInstallments; i++)
            {
                installments.Add(new PurchaseInstallment
                {
                    NumberInstallment = i + 1,
                    Value = valueInstallment,
                    DueDate = purchase.PurchaseDate.AddMonths(i),
                    IsPaid = false,
                    PurchaseId = purchase.Id,
                    Purchase = purchase
                });
            }

            // Ajustar diferença final se sobrar centavos
            var totalInstallments = installments.Sum(x => x.Value);
            var diff = purchase.Value - totalInstallments;
            if (diff != 0 && installments.Count > 0)
                installments[^1].Value += diff;

            return installments;
        }
    }
}
