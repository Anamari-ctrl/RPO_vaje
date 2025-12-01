namespace WebStore.Entities.Models
{
    public class Rating
    {
        public Guid RatingId { get; set; }

        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        public int RatingValue { get; set; }
    }
}
