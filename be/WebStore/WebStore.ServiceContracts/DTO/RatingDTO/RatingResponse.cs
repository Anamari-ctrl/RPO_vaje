using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.RatingDTO
{
    public class RatingResponse
    {
        public Guid RatingId { get; set; }

        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        public int RatingValue { get; set; }

        public string? Comment { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public RatingUpdateRequest ToRatingUpdateRequest()
        {
            return new RatingUpdateRequest()
            {
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

    public static class RatingResponseExtensions
    {
        public static RatingResponse ToRatingResponse(this Rating rating)
        {
            return new RatingResponse()
            {
                RatingId = rating.RatingId,
                ProductId = rating.ProductId,
                UserId = rating.UserId,
                RatingValue = rating.RatingValue,
                Comment = rating.Comment,
                CreatedBy = rating.CreatedBy,
                Created = rating.Created,
                Updated = rating.Updated
            };
        }
    }
}
