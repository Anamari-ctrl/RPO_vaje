using WebStore.Entities.Enums;

namespace WebStore.Entities.Models
{
    public class Category : BaseEntity
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public CategoryType CategoryType { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}
