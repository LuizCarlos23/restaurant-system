using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.DTOs
{
    public class IngredientDTO
    {
        [Key]
        public long? Id { get; set; }

        [Required(ErrorMessage = "Nome do ingrediente é obrigatório.")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Preço do ingrediente é obrigatório.")]
        [Display(Name = "Preço")]
        public decimal Price { get; set; }
    }
}
