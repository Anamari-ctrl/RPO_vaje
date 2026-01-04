using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities.Models
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }

        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        public decimal PriceAtPurchase { get; set; }

        public string? ProductName { get; set; }

        public string? ShortDescription { get; set; }
    }
}
