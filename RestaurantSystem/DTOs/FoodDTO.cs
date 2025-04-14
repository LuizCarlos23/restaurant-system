using RestaurantSystem.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantSystem.DTOs
{
    public class FoodDTO
    {
        [Key]
        public long? Id { get; set; }

        [Required(ErrorMessage = "Nome da comida é obrigatório.")]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descrição da comida é obrigatório.")]
        [DataType(DataType.Text)]
        [DisplayName("Descrição")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Preço da comida é obrigatório.")]
        [DisplayName("Preço")]
        public decimal BasePrice { get; set; }

        public IFormFile? Image { get; set; }

        public string? ImageMimeType { get; set; }

        [AllowNull]
        public ICollection<long>? OptionalIngredients { get; set; }

        [AllowNull]
        public ICollection<long>? ExclusiveIngredients { get; set; }
    }
}
