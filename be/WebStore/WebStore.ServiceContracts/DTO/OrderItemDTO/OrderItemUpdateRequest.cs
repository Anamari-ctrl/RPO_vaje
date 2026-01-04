using System.ComponentModel.DataAnnotations;
using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.OrderItemDTO
{
    public class OrderItemUpdateRequest
    {
        [Required(ErrorMessage = "Product Id is required!")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Order Id is required!")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "Quantity is required!")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price  is required!")]
        public decimal PriceAtPurchase { get; set; }

        public OrderItem ToOrderItem()
        {
            return new OrderItem()
            {
                OrderId = OrderId,
                ProductId = ProductId,
                Quantity = Quantity,
                PriceAtPurchase = PriceAtPurchase
            };
        }
    }
}
