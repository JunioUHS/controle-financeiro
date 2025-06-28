using ControleFinanceiro.Api.DTOs.Reports;
using ControleFinanceiro.Api.Services.Contracts;
using ControleFinanceiro.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("expenses-by-category")]
    public async Task<IActionResult> GetExpensesByCategory([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reportService.GetExpensesByCategoryAsync(userId!, start, end);
        return Ok(ApiResponse<IEnumerable<ExpenseByCategoryReportDto>>.Ok(result.Value));
    }

    [HttpGet("balance-summary")]
    public async Task<IActionResult> GetBalanceSummary([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reportService.GetBalanceSummaryAsync(userId!, start, end);
        return Ok(ApiResponse<BalanceSummaryReportDto>.Ok(result.Value));
    }

    [HttpGet("credit-card-transactions")]
    public async Task<IActionResult> GetCreditCardTransactions([FromQuery] int creditCardId, [FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reportService.GetCreditCardTransactionsAsync(userId!, creditCardId, start, end);
        return Ok(ApiResponse<IEnumerable<CreditCardTransactionReportDto>>.Ok(result.Value));
    }

    [HttpGet("balance-evolution")]
    public async Task<IActionResult> GetBalanceEvolution([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reportService.GetBalanceEvolutionAsync(userId!, start, end);
        return Ok(ApiResponse<IEnumerable<BalanceEvolutionReportDto>>.Ok(result.Value));
    }

    //[HttpGet("expenses-by-category/export")]
    //public async Task<IActionResult> ExportExpensesByCategory([FromQuery] DateTime month, [FromQuery] string type)
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var fileResult = await _reportService.ExportExpensesByCategoryAsync(userId!, month, type);
    //    return File(fileResult.Content, fileResult.ContentType, fileResult.FileName);
    //}

    // Repita para outros relat�rios/exporta��es
}