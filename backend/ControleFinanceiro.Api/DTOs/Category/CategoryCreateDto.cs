using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Api.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Nome � obrigat�rio.")]
        public string? Name { get; set; }
    }
}