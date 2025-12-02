using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Entities.Identity;

namespace WebStore.Entities.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public int OrderNumber { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}
