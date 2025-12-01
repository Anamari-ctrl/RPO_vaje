namespace WebStore.Entities.Models
{
    public class Product : BaseEntity
    {
        public Guid ProductId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid BranchId { get; set; }

        public int ProductNumber { get; set; }

        public string? ProductName { get; set; }

        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        public double Price { get; set; }

        public bool IsAvailable { get; set; }

        public int Warranty { get; set; }

        public string ManufacturerPageUrl { get; set; } = string.Empty;

        public string TechnicalDetails { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

    }
}
