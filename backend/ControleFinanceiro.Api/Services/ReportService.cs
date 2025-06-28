using ControleFinanceiro.Api.DTOs.Reports;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Results;
using ControleFinanceiro.Api.Services.Contracts;

namespace ControleFinanceiro.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly IAccountPayableRepository _accountPayableRepository;
        private readonly IAccountReceivableRepository _accountReceivableRepository;
        private readonly ICreditCardPurchaseRepository _creditCardPurchaseRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ReportService(
            IAccountPayableRepository accountPayableRepository,
            IAccountReceivableRepository accountReceivableRepository,
            ICreditCardPurchaseRepository creditCardPurchaseRepository,
            ICategoryRepository categoryRepository)
        {
            _accountPayableRepository = accountPayableRepository;
            _accountReceivableRepository = accountReceivableRepository;
            _creditCardPurchaseRepository = creditCardPurchaseRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<IEnumerable<ExpenseByCategoryReportDto>>> GetExpensesByCategoryAsync(string userId, DateTime start, DateTime end)
        {
            var payables = await _accountPayableRepository.GetAllAsync(userId, start, end);

            var result = payables
                .GroupBy(p => p.Category.Name)
                .Select(g => new ExpenseByCategoryReportDto
                {
                    Category = g.Key,
                    Total = g.Sum(x => x.Value)
                })
                .ToList();

            return Result<IEnumerable<ExpenseByCategoryReportDto>>.Success(result);
        }

        public async Task<Result<BalanceSummaryReportDto>> GetBalanceSummaryAsync(string userId, DateTime start, DateTime end)
        {
            var receivables = await _accountReceivableRepository.GetAllAsync(userId);
            var payables = await _accountPayableRepository.GetAllAsync(userId);

            var totalReceivable = receivables.Where(r => r.ReceiptDate >= start && r.ReceiptDate <= end).Sum(r => r.Value);
            var totalPayable = payables.Where(p => p.DueDate >= start && p.DueDate <= end).Sum(p => p.Value);

            var dto = new BalanceSummaryReportDto
            {
                TotalReceivable = totalReceivable,
                TotalPayable = totalPayable,
                Balance = totalReceivable - totalPayable
            };

            return Result<BalanceSummaryReportDto>.Success(dto);
        }

        public async Task<Result<IEnumerable<CreditCardTransactionReportDto>>> GetCreditCardTransactionsAsync(string userId, int creditCardId, DateTime start, DateTime end)
        {
            var purchases = await _creditCardPurchaseRepository.GetAllAsync(userId, new DTOs.CreditCardPurchase.CreditCardPurchaseFilterDto
            {
                CreditCardId = creditCardId,
                StartDate = start,
                EndDate = end
            });

            var result = purchases.Select(p => new CreditCardTransactionReportDto
            {
                CardName = p.CreditCard.Name,
                PurchaseDate = p.PurchaseDate,
                Description = p.Description,
                Value = p.Value,
                Category = p.Category.Name
            }).ToList();

            return Result<IEnumerable<CreditCardTransactionReportDto>>.Success(result);
        }

        public async Task<Result<IEnumerable<BalanceEvolutionReportDto>>> GetBalanceEvolutionAsync(string userId, DateTime start, DateTime end)
        {
            var receivables = await _accountReceivableRepository.GetAllAsync(userId);
            var payables = await _accountPayableRepository.GetAllAsync(userId);

            var dates = Enumerable.Range(0, (end - start).Days + 1)
                .Select(offset => start.AddDays(offset))
                .ToList();

            var result = new List<BalanceEvolutionReportDto>();
            decimal runningBalance = 0;

            foreach (var date in dates)
            {
                runningBalance += receivables.Where(r => r.ReceiptDate.Date == date.Date).Sum(r => r.Value);
                runningBalance -= payables.Where(p => p.DueDate.Date == date.Date).Sum(p => p.Value);

                result.Add(new BalanceEvolutionReportDto
                {
                    Date = date,
                    Balance = runningBalance
                });
            }

            return Result<IEnumerable<BalanceEvolutionReportDto>>.Success(result);
        }

        //public async Task<FileResultDto> ExportExpensesByCategoryAsync(string userId, DateTime month, string type)
        //{
        //    var report = await GetExpensesByCategoryAsync(userId, month);
        //    var lines = new List<string> { "Categoria,Total" };
        //    lines.AddRange(report.Value.Select(r => $"{r.Category},{r.Total}"));
        //    var content = System.Text.Encoding.UTF8.GetBytes(string.Join("\n", lines));

        //    return new FileResultDto
        //    {
        //        Content = content,
        //        ContentType = "text/csv",
        //        FileName = $"despesas-por-categoria-{month:yyyy-MM}.csv"
        //    };
        //}
    }
}