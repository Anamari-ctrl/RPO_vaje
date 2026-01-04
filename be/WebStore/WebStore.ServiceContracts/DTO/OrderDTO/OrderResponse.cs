using WebStore.Entities.Models;
using WebStore.ServiceContracts.DTO.OrderItemDTO;

namespace WebStore.ServiceContracts.DTO.OrderDTO
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }

        public Guid? UserId { get; set; }

        public string? OrderStatus { get; set; }

        public int OrderNumber { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public bool IsActive { get; set; }

        public List<OrderItemResponse> OrderItems { get; set; } = [];

        public OrderUpdateRequest ToOrderUpdateRequest()
        {
            return new OrderUpdateRequest()
            {

            };
        }
    }

    public static class OrderResponseExtensions
    {
        public static OrderResponse ToOrderResponse(this Order order)
        {
            return new OrderResponse()
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderStatus = order.OrderStatus,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Address = order.Address,
                City = order.City,
                Country = order.Country,
                PostalCode = order.PostalCode,
                IsActive = order.IsActive,
                OrderItems = order.OrderItems.Select(x => x.ToOrderItemResponse()).ToList()
            };
        }
    }
}
