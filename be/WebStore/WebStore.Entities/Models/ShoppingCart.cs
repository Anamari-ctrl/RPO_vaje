using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Entities.Identity;

namespace WebStore.Entities.Models
{
    public class ShoppingCart
    {
        [Key]
        public Guid CartId { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double DDV { get; set; }

        public double DeliveryCost { get; set; }

        public ICollection<ShoppingCartItem> Items { get; set; } = [];
    }
}
