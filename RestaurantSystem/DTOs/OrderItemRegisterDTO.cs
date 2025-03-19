using RestaurantSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantSystem.DTOs
{
    public class OrderItemRegisterDTO
    {
        [Required]
        public long FoodID { get; set; }


        [Required]
        public int Quantity { get; set; }

        [AllowNull]
        public long[]? SelectedOptionalIngredients { get; set; }
    }
}
