using ControleFinanceiro.Api.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ControleFinanceiro.Api.Extensions
{
    public static class ModelStateExtensions
    {
        public static IEnumerable<ApiError> ToApiErrors(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(error => new ApiError
                {
                    Field = x.Key,
                    Message = error.ErrorMessage
                }))
                .ToList();
        }
    }
}
