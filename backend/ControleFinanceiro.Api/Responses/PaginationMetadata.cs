namespace ControleFinanceiro.Api.Responses
{
    public class PaginationMetadata
    {
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

}
