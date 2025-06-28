namespace ControleFinanceiro.Api.DTOs.Reports
{
    public class FileResultDto
    {
        public byte[] Content { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public string FileName { get; set; } = null!;
    }
}