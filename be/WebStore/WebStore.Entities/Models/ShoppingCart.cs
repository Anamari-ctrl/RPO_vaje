namespace WebStore.Entities.Models
{
    public class ShoppingCart
    {
        public Guid CartId { get; set; }

        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double DDV { get; set; }

        public double DeliveryCost { get; set; }
    }
}
