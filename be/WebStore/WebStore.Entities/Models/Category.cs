using System.ComponentModel.DataAnnotations;

namespace WebStore.Entities.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public Guid CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
