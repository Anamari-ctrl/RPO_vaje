using System.ComponentModel.DataAnnotations;
using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.BranchDTO
{
    public class BranchAddRequest
    {
        [Required(ErrorMessage = "Branch name is required!")]
        public string? BranchName { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "City is required!")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Country is required!")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Postal code is required!")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Phone is required!")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Responsible person is required!")]
        public string? Responsible { get; set; }

        [Required(ErrorMessage = "Opening hours are required!")]
        public string? OpeningHours { get; set; }

        [Required(ErrorMessage = "Image is required!")]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public Branch ToBranch()
        {
            return new Branch()
            {
                BranchName = BranchName,
                Address = Address,
                City = City,
                Country = Country,
                PostalCode = PostalCode,
                Phone = Phone,
                Responsible = Responsible,
                OpeningHours = OpeningHours,
                ImageUrl = ImageUrl,
                IsActive = IsActive
            };
        }
    }
}
