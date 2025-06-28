namespace ControleFinanceiro.Api.DTOs.AccountReceivable
{
    public class AccountReceivableUpdateDto : AccountReceivableCreateDto
    {
        public bool IsReceived { get; set; }
    }
}