using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.OrderDTO
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public string? OrderStatus { get; set; }

        public int OrderNumber { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public bool IsActive { get; set; }

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
            };
        }
    }
}
