using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebStore.Entities.Identity;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.AuthDTO;
using WebStore.Services.Helpers;

namespace WebStore.API.Endpoints.v1
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/auth/register", Register);
            app.MapPost("api/v1/auth/login", Login);
            app.MapPost("api/v1/auth/refresh", RefreshToken).RequireAuthorization();
            app.MapGet("api/v1/auth/logout", Logout).RequireAuthorization();
            app.MapPost("api/v1/auth/reset-password", ResetPassword);
            app.MapPost("api/v1/auth/forgot-password", ForgotPassword);
        }

        [AllowAnonymous]
        public static async Task<IResult> Register(RegisterResponse registerDTO,
                                                   UserManager<ApplicationUser> userManager,
                                                   SignInManager<ApplicationUser> signInManager,
                                                   RoleManager<ApplicationRole> roleManager,
                                                   IJwtService jwtService)
        {
            if (!ValidationHelper.IsModelValid(registerDTO, out List<ValidationResult> errors))
            {
                var errorsMessages = string.Join("|", errors.Select(x => x.ErrorMessage));
                return Results.Problem(errorsMessages);
            }

            var userWithEmail = await userManager.FindByEmailAsync(registerDTO.Email!);
            if (userWithEmail != null)
            {
                return Results.Problem($"Email {registerDTO.Email} is already taken!");
            }

            ApplicationUser user = new()
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            bool userRoleExists = await roleManager.RoleExistsAsync("User");

            if (!userRoleExists)
            {
                return Results.Problem("User role does not exists. Check backend configuration!");
            }

            IdentityResult result = await userManager.CreateAsync(user, registerDTO.Password!);

            if (!result.Succeeded)
            {
                return Results.BadRequest("Validation was OK, but the registration was not successful!");
            }

            await userManager.AddToRoleAsync(user, "User");

            await signInManager.SignInAsync(user, isPersistent: false);

            var authenticationResponse = jwtService.CreateJwtToken(user, "User");

            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;

            await userManager.UpdateAsync(user);

            authenticationResponse.IsAdmin = await userManager.IsInRoleAsync(user, "Admin");

            return Results.Ok(authenticationResponse);
        }

        [AllowAnonymous]
        public static async Task<IResult> Login(LoginResponse loginDTO,
                                                SignInManager<ApplicationUser> signInManager,
                                                UserManager<ApplicationUser> userManager,
                                                IJwtService jwtService)
        {
            if (!ValidationHelper.IsModelValid(loginDTO, out List<ValidationResult> errors))
            {
                var errorMessages = string.Join("|", errors.Select(x => x.ErrorMessage));

                return Results.Problem(errorMessages);
            }

            var result = await signInManager.PasswordSignInAsync(loginDTO.Email,
                                                                 loginDTO.Password,
                                                                 isPersistent: true,
                                                                 lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Results.Problem("Username or password are not correct!");
            }

            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return Results.NoContent();
            }

            string role = "Admin";
            bool isUserAdmin = await userManager.IsInRoleAsync(user, role);
            if (!isUserAdmin)
            {
                role = "User";
            }

            var authenticationResponse = jwtService.CreateJwtToken(user, role);
            authenticationResponse.IsAdmin = isUserAdmin;

            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;

            await userManager.UpdateAsync(user);

            return Results.Ok(authenticationResponse);
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

            string role = "Admin";
            bool isUserAdmin = await userManager.IsInRoleAsync(user, role);
            if (!isUserAdmin)
            {
                role = "User";
            }

            AuthenticationResponse response = jwtService.CreateJwtToken(user, role);

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpirationDateTime = response.RefreshTokenExpirationDateTime;

            await userManager.UpdateAsync(user);

            return Results.Ok(response);
        }

        public static async Task<IResult> Logout(SignInManager<ApplicationUser> signInManager)
        {
            await signInManager.SignOutAsync();

            return Results.Ok();
        }

        public static async Task<IResult> ForgotPassword(ForgotPasswordDTO forgotPassword,
                                                         UserManager<ApplicationUser> userManager)
        {
            if (!ValidationHelper.IsModelValid(forgotPassword, out List<ValidationResult> errors))
            {
                Dictionary<string, string[]> validationErrors = new()
                {
                    {"", errors.Select(x=>x.ErrorMessage).ToArray()! }
                };

                return Results.ValidationProblem(validationErrors);
            }

            ApplicationUser? user = await userManager.FindByEmailAsync(forgotPassword.Email!);

            if (user == null)
            {
                return Results.BadRequest("Email was not found!");
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            Dictionary<string, string> param = new()
            {
                {"token", token},
                {"email", forgotPassword.Email!},
            };

            var callback = QueryHelpers.AddQueryString(forgotPassword.ClientUri!, param!);

            return Results.Ok(callback);
        }

        public static async Task<IResult> ResetPassword(ResetPasswordDTO resetPasswordDTO,
                                                        UserManager<ApplicationUser> userManager,
                                                        ClaimsPrincipal user)
        {
            if (!ValidationHelper.IsModelValid(resetPasswordDTO, out List<ValidationResult> errors))
            {
                Dictionary<string, string[]> validationErrors = new()
                {
                    {"", errors.Select(x=>x.ErrorMessage).ToArray()! }
                };

                return Results.ValidationProblem(validationErrors);
            }

            ApplicationUser? currentUser;

            if (!string.IsNullOrEmpty(resetPasswordDTO.Email))
            {
                currentUser = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
            }
            else
            {
                currentUser = await userManager.GetUserAsync(user);
            }

            if (currentUser == null)
            {
                return Results.NotFound("User was not found!");
            }

            IdentityResult? result = await userManager.ResetPasswordAsync(currentUser,
                                                                          resetPasswordDTO.Token!,
                                                                          resetPasswordDTO.NewPassword!);

            if (!result.Succeeded)
            {
                return Results.BadRequest(new { Errors = result.Errors.Select(x => x.Description) });
            }

            return Results.Ok("Your password was successfuly changed!");
        }
    }
}
