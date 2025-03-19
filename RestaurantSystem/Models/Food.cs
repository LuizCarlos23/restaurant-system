using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantSystem.Models
{
    public class Food
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Descrição")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Preço")]
        public decimal BasePrice { get; set; }

        [AllowNull]
        public ICollection<Ingredient>? OptionalIngredients { get; set; }
    }
}
