using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.DTOs
{
    public class OrderDto
    {
        [Required]
        public ICollection<OrderItemDTO> OrderedItems { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }
}
