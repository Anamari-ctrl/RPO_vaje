namespace WebStore.Entities.Models
{
    public class Review : BaseEntity
    {
        public Guid ReviewId { get; set; }

        public string Author { get; set; } = string.Empty;

        public bool IsAuthorVisible { get; set; }

        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        public string Comment { get; set; } = string.Empty;

        public int Rating { get; set; }
    }
}
