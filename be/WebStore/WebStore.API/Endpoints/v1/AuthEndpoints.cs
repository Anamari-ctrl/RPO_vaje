using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebStore.Entities.Identity;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.Account;
using WebStore.Services.Helpers;

namespace WebStore.API.Endpoints.v1
{
    public static class AuthEndpoints
    {
        public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/auth/register", Register);
            app.MapPost("api/v1/auth/login", Login);
            app.MapPost("api/v1/auth/refresh", RefreshToken).RequireAuthorization();
            app.MapGet("api/v1/auth/logout", Logout).RequireAuthorization();
        }

        [AllowAnonymous]
        public static async Task<IResult> Register(RegisterResponse registerDTO,
                                                   UserManager<ApplicationUser> userManager,
                                                   SignInManager<ApplicationUser> signInManager,
                                                   IJwtService jwtService)
        {
            if (ValidationHelper.IsModelValid(registerDTO, out List<ValidationResult> errors))
            {
                var userWithEmail = await userManager.FindByEmailAsync(registerDTO.Email!);
                if (userWithEmail != null)
                {
                    return Results.Problem($"Email {registerDTO.Email} is already taken!");
                }

                ApplicationUser user = new()
                {
                    FullName = registerDTO.FullName,
                    Email = registerDTO.Email,
                    UserName = registerDTO.Email,
                };

                IdentityResult result = await userManager.CreateAsync(user, registerDTO.Password!);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    var authenticationResponse = jwtService.CreateJwtToken(user);

                    user.RefreshToken = authenticationResponse.RefreshToken;
                    user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;

                    await userManager.UpdateAsync(user);

                    return Results.Ok(authenticationResponse);
                }

                return Results.BadRequest("Validation was OK, but the registration was not successful!");
            }

            var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));
            return Results.Problem(errorsMessages);
        }

        [AllowAnonymous]
        public static async Task<IResult> Login(LoginResponse loginDTO,
                                                SignInManager<ApplicationUser> signInManager,
                                                UserManager<ApplicationUser> userManager,
                                                IJwtService jwtService)
        {
            if (ValidationHelper.IsModelValid(loginDTO, out List<ValidationResult> errors))
            {
                var result = await signInManager.PasswordSignInAsync(loginDTO.Email,
                                                                     loginDTO.Password,
                                                                     isPersistent: true,
                                                                     lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(loginDTO.Email);
                    if (user == null)
                    {
                        return Results.NoContent();
                    }

                    var authenticationResponse = jwtService.CreateJwtToken(user);

                    user.RefreshToken = authenticationResponse.RefreshToken;
                    user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;

                    await userManager.UpdateAsync(user);

                    return Results.Ok(authenticationResponse);
                }

                return Results.Problem("Username or password are not correct!");
            }

            var errorMessages = string.Join("|", errors.Select(x => x.ErrorMessage));

            return Results.Problem(errorMessages);
        }

        public static async Task<IResult> RefreshToken(TokenModel tokenModel,
                                                       IJwtService jwtService,
                                                       UserManager<ApplicationUser> userManager)
        {
            if (tokenModel == null)
            {
                return Results.BadRequest("Invalid client request!");
            }

            ClaimsPrincipal? principal = jwtService.GetPrincipalFromJwtToken(tokenModel.Token);

            if (principal == null)
            {
                return Results.BadRequest("Invalid jwt access token");
            }

            string? email = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            ApplicationUser? user = await userManager.FindByEmailAsync(email!);

            if (user == null || user.RefreshToken != tokenModel.RefreshToken
                || user.RefreshTokenExpirationDateTime <= DateTime.Now)
            {
                return Results.BadRequest("Invalid refresh token");
            }

            AuthenticationResponse response = jwtService.CreateJwtToken(user);

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpirationDateTime = response.RefreshTokenExpirationDateTime;

            await userManager.UpdateAsync(user);

            return Results.Ok(response);
        }

        public static async Task<IResult> Logout(SignInManager<ApplicationUser> signInManager)
        {
            await signInManager.SignOutAsync();

            return Results.NoContent();
        }
    }
}
