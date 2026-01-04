using System.Security.Claims;
using WebStore.Entities.Identity;
using WebStore.ServiceContracts.DTO.AuthDTO;

namespace WebStore.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user,string role);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
