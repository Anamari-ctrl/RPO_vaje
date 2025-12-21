using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities.Models
{
    public class Review : BaseEntity
    {
        [Key]
        public Guid ReviewId { get; set; }

        public string? Author { get; set; }

        public bool IsAuthorVisible { get; set; }

        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public string? Comment { get; set; }

        public int Rating { get; set; }
    }
}
