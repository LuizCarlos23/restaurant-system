using RestaurantSystem.Enums;
using RestaurantSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.DTOs
{
    public class CustomerOrderDto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
