using WebStore.Entities.Models;

namespace WebStore.ServiceContracts.DTO.BranchDTO
{
    public class BranchResponse
    {
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

    public static class BranchResponseExtensions
    {
        public static BranchResponse ToBranchResponse(this Branch branch)
        {
            return new BranchResponse()
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                Address = branch.Address,
                City = branch.City,
                Country = branch.Country,
                PostalCode = branch.PostalCode,
                Phone = branch.Phone,
                Responsible = branch.Responsible,
                OpeningHours = branch.OpeningHours,
                ImageUrl = branch.ImageUrl
            };
        }
    }
}
