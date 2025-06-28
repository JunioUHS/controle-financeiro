using System.Net;
using System.Text.Json;
using ControleFinanceiro.Api.Responses;

namespace ControleFinanceiro.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro n�o tratado");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = ApiResponse<object>.Fail("Erro interno do servidor.");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}