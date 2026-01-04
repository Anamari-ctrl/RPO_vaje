using System.ComponentModel.DataAnnotations;
using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.RatingDTO
{
    public class RatingUpdateRequest
    {
        [Required(ErrorMessage = "Rating id is required!")]
        public Guid RatingId { get; set; }

        [Required(ErrorMessage = "Product id is required!")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "User id is required!")]
        public Guid UserId { get; set; }

        public int RatingValue { get; set; }

        public string? Comment { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public Rating ToRating()
        {
            return new Rating()
            {
                RatingId = RatingId,
                ProductId = ProductId,
                UserId = UserId,
                RatingValue = RatingValue,
                Comment = Comment,
                CreatedBy = CreatedBy,
                Created = Created,
                Updated = Updated
            };
        }
    }
}
