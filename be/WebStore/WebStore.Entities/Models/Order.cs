namespace WebStore.Entities.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public int OrderNumber { get; set; }

        public Guid UserId { get; set; }

        public Guid CartId { get; set; }

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;
    }
}
