using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public Guid CartItemId { get; set; }

        public Guid CartId { get; set; }

        [ForeignKey("CartId")]
        public ShoppingCart? ShoppingCart { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
