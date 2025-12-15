using System.ComponentModel.DataAnnotations;

namespace WebStore.Entities.Models
{
    public class Branch : BaseEntity
    {
        [Key]
        public Guid BranchId { get; set; }

        public string? BranchName { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public string? Phone { get; set; }

        public string? Responsible { get; set; }

        public string? OpeningHours { get; set; }

        public string? ImageUrl { get; set; }
    }
}
