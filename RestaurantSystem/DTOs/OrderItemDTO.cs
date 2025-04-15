using RestaurantSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantSystem.DTOs
{
    public class OrderItemDTO
    {
        [Required]
        public Food Food { get; set; }

        [AllowNull]
        public ICollection<Ingredient>? OptionalIngredientsSelected { get; set; }

        [AllowNull]
        public Ingredient? ExclusiveIngredientSelected { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
    }
}
