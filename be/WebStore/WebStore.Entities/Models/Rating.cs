using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities.Models
{
    public class Rating
    {
        [Key]
        public Guid RatingId { get; set; }

        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public int RatingValue { get; set; }
    }
}
