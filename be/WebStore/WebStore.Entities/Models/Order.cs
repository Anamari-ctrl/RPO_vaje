using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Entities.Identity;

namespace WebStore.Entities.Models
{
    public class Order : BaseEntity
    {
        [Key]
        public Guid OrderId { get; set; }

        public int OrderNumber { get; set; }

        public string? OrderStatus { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public List<OrderItem> OrderItems { get; set; } = [];
    }
}
