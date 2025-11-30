using Microsoft.AspNetCore.Identity;

namespace WebStore.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
    }
}
