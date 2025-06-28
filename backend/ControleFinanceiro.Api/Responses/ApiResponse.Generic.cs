namespace ControleFinanceiro.Api.Responses
{
    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; protected set; }

        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new() { Success = true, Data = data, Message = message };

        public static new ApiResponse<T> Fail(string message) =>
            new() { Success = false, Message = message };

        public static new ApiResponse<T> Fail(IEnumerable<ApiError> errors, string? message = null) =>
            new() { Success = false, Errors = errors, Message = message };
    }
}
