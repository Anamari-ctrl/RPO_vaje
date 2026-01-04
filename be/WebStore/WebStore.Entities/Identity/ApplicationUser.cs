using Microsoft.AspNetCore.Identity;

namespace WebStore.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;

        public string? Country { get; set; } = string.Empty;

        public string? PostalCode { get; set; } = string.Empty;

        public string? RefreshToken { get; set; } = string.Empty;

        public DateTime? RefreshTokenExpirationDateTime { get; set; }
        public string? Role { get; set; } = string.Empty;
    }
}
