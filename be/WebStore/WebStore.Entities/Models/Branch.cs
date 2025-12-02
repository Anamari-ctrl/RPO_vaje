using System.ComponentModel.DataAnnotations;

namespace WebStore.Entities.Models
{
    public class Branch : BaseEntity
    {
        [Key]
        public Guid BranchId { get; set; }

        public string BranchName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Responsible { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
