namespace ControleFinanceiro.Api.DTOs.Reports
{
    public class ExpenseByCategoryReportDto
    {
        public string Category { get; set; } = null!;
        public decimal Total { get; set; }
    }
}