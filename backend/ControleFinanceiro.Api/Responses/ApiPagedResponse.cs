namespace ControleFinanceiro.Api.Responses
{
    public class ApiPagedResponse<T> : ApiResponse<IEnumerable<T>>
    {
        public PaginationMetadata Meta { get; protected set; } = new();

        public static ApiPagedResponse<T> Ok(IEnumerable<T> data, int totalItems, int page, int pageSize, string? message = null)
        {
            return new ApiPagedResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                Meta = new PaginationMetadata
                {
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                }
            };
        }
    }
}
