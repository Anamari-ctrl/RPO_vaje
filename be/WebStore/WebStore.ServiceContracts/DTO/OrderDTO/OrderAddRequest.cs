using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.OrderDTO
{
    public class OrderAddRequest
    {
        public Guid UserId { get; set; }

        public string? OrderStatus { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public Order ToOrder()
        {
            return new Order()
            {
                UserId = UserId,
                OrderStatus = OrderStatus,
                TotalAmount = TotalAmount,
                Address = Address,
                City = City,
                Country = Country,
                PostalCode = PostalCode,
            };
        }
    }
}
