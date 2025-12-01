using System.Security.Claims;
using WebStore.Entities.Identity;
using WebStore.ServiceContracts.DTO.Account;

namespace WebStore.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
