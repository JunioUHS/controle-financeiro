namespace ControleFinanceiro.Api.Responses
{
    public class ApiResponse
    {
        public bool Success { get; protected set; }
        public string? Message { get; protected set; }
        public IEnumerable<ApiError>? Errors { get; protected set; }

        public static ApiResponse Ok(string? message = null) =>
            new() { Success = true, Message = message };

        public static ApiResponse Fail(string message) =>
            new() { Success = false, Message = message };

        public static ApiResponse Fail(IEnumerable<ApiError> errors, string? message = null) =>
            new() { Success = false, Errors = errors, Message = message };
    }
}