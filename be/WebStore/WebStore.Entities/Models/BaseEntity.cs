namespace WebStore.Entities.Models
{
    public abstract class BaseEntity
    {
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }
}
