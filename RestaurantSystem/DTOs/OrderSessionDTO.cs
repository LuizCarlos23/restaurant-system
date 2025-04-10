using RestaurantSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.DTOs
{
    public class OrderSessionDTO
    {
        [Required]
        public ICollection<OrderItemSessionDTO> OrderItems { get; set; }
    }
}
