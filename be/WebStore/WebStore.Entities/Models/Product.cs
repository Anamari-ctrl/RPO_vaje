using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public Guid ProductId { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public Guid GenreId { get; set; }

        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }

        public Guid BranchId { get; set; }

        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

        public int ProductNumber { get; set; }

        public string? ProductName { get; set; }

        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public int Warranty { get; set; }

        public string? ManufacturerPageUrl { get; set; }

        public string? TechnicalDetails { get; set; }

        public string? ImageUrl { get; set; }

        public List<Rating> Ratings { get; set; } = [];
    }
}
