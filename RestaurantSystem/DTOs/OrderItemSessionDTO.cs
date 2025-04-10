using RestaurantSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantSystem.DTOs
{
    public class OrderItemSessionDTO
    {

        [Required]
        public long FoodId { get; set; }

        [AllowNull]
        public IEnumerable<long>? OptionalIngredientsSelectedIds { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
