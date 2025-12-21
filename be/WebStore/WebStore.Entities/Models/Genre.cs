using System.ComponentModel.DataAnnotations;

namespace WebStore.Entities.Models
{
    public class Genre : BaseEntity
    {
        [Key]
        public Guid GenreId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
