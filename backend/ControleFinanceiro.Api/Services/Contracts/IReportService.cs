using ControleFinanceiro.Api.DTOs.Reports;
using ControleFinanceiro.Api.Results;

public interface IReportService
{
    Task<Result<IEnumerable<ExpenseByCategoryReportDto>>> GetExpensesByCategoryAsync(string userId, DateTime start, DateTime end);
    Task<Result<BalanceSummaryReportDto>> GetBalanceSummaryAsync(string userId, DateTime start, DateTime end);
    Task<Result<IEnumerable<CreditCardTransactionReportDto>>> GetCreditCardTransactionsAsync(string userId, int creditCardId, DateTime start, DateTime end);
    Task<Result<IEnumerable<BalanceEvolutionReportDto>>> GetBalanceEvolutionAsync(string userId, DateTime start, DateTime end);

    //Task<FileResultDto> ExportExpensesByCategoryAsync(string userId, DateTime month, string type);
}