using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.OrderItemDTO
{
    public class OrderItemResponse
    {
        public Guid OrderItemId { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal PriceAtPurchase { get; set; }

        public string? ProductName { get; set; }

        public string? ShortDescription { get; set; }

        public decimal Price { get; set; }
    }

    public static class OrderItemResponseExtensions
    {
        public static OrderItemResponse ToOrderItemResponse(this OrderItem orderItem)
        {
            return new OrderItemResponse()
            {
                OrderItemId = orderItem.OrderItemId,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                //PriceAtPurchase = orderItem.PriceAtPurchase,
                //ProductName = orderItem.Product!.ProductName,
                //ShortDescription = orderItem.Product.ShortDescription,
                //Price = orderItem.Product.Price
            };
        }
    }
}
