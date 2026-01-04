using Microsoft.AspNetCore.Identity;

namespace WebStore.Entities.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
            
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {

        }
    }
}
