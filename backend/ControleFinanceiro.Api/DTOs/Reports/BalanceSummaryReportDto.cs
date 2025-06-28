namespace ControleFinanceiro.Api.DTOs.Reports
{
    public class BalanceSummaryReportDto
    {
        public decimal TotalReceivable { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal Balance { get; set; }
    }
}