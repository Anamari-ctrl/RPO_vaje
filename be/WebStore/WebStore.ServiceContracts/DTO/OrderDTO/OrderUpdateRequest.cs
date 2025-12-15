using System.ComponentModel.DataAnnotations;
using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.OrderDTO
{
    public class OrderUpdateRequest
    {
        [Required(ErrorMessage = "Order id can't be blank!")]
        public Guid OrderId { get; set; }

        public string? OrderStatus { get; set; }

        public int OrderNumber { get; set; }

        public bool IsActive { get; set; }

        public Order ToOrder()
        {
            return new Order()
            {
            };
        }
    }
}
