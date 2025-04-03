using System.ComponentModel.DataAnnotations;
using RestaurantSystem.Enums;

namespace RestaurantSystem.Models
{
    public class Order
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public ICollection<OrderItem> OrderedItems { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
