using RestaurantSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantSystem.DTOs
{
    public class OrderItemSessionDTO
    {

        [Required]
        public Food Food { get; set; }

        [AllowNull]
        public ICollection<IngredientDTO>? OptionalIngredientsSelected { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
    }
}
