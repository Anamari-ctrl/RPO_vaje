using WebStore.Entities.Identity;

namespace WebStore.ServiceContracts.DTO.AuthDTO
{
    public class UserDataResponse
    {
        public Guid UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public string? PhoneNumber { get; set; }

        public static implicit operator UserDataResponse(ApplicationUser appUser)
        {
            return new UserDataResponse()
            {
                UserId = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                Address = appUser.Address,
                City = appUser.City,
                Country = appUser.Country,
                PostalCode = appUser.PostalCode,
                PhoneNumber = appUser.PhoneNumber,
            };
        }

    }
}
