using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantSystem.Models
{
    public class OrderItem
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Food Food { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public ICollection<Ingredient>? OptionalIngredientsSelected { get; set; }
        public Ingredient? ExclusiveIngredientSelected { get; set; }

    }
}
