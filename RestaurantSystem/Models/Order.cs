using System.ComponentModel.DataAnnotations;

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
    }
}
