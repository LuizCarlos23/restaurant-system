using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantSystem.Models
{
    public class Ingredient
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Preço")]
        public decimal Price { get; set; }

        [AllowNull]
        public virtual ICollection<Food>? Foods { get; set; }
    }
}
