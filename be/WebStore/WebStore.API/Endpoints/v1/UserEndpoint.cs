using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebStore.Entities.Identity;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.AuthDTO;
using WebStore.ServiceContracts.DTO.OrderDTO;

namespace WebStore.API.Endpoints.v1
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/users/me", GetUserData).RequireAuthorization();
            app.MapPut("api/v1/users/me", UpdateUserData).RequireAuthorization();
            app.MapGet("api/v1/users/orderHistory", GetUserOrderHistory).RequireAuthorization();
        }

        public static async Task<IResult> GetUserData(string? userId,
                                                      UserManager<ApplicationUser> userManager,
                                                      ClaimsPrincipal userPrincipal)
        {
            ApplicationUser? user;

            if (!string.IsNullOrEmpty(userId))
            {
                user = await userManager.FindByIdAsync(userId);
            }
            else
            {
                user = await userManager.GetUserAsync(userPrincipal);
            }

            if (user == null)
            {
                return Results.NotFound();
            }

            UserDataResponse userData = user;

            return Results.Ok(userData);
        }

        public static async Task<IResult> UpdateUserData(UserUpdateRequest? userUpdateRequest,
                                                         UserManager<ApplicationUser> userManager,
                                                         ClaimsPrincipal userPrincipal)
        {
            if (userUpdateRequest == null)
                return Results.BadRequest("Request body was empty.");

            try
            {
                // Always get the user from the authenticated principal
                var appUser = await userManager.GetUserAsync(userPrincipal);

                if (appUser == null)
                    return Results.NotFound("Authenticated user not found.");

                // Update allowed fields
                appUser.FirstName = userUpdateRequest.FirstName;
                appUser.LastName = userUpdateRequest.LastName;
                appUser.Address = userUpdateRequest.Address;
                appUser.City = userUpdateRequest.City;
                appUser.Country = userUpdateRequest.Country;
                appUser.PostalCode = userUpdateRequest.PostalCode;
                appUser.PhoneNumber = userUpdateRequest.PhoneNumber;

                // Handle email change properly
                if (!string.Equals(appUser.Email, userUpdateRequest.Email, StringComparison.OrdinalIgnoreCase))
                {
                    appUser.Email = userUpdateRequest.Email;
                    appUser.UserName = userUpdateRequest.Email;
                    appUser.NormalizedEmail = userUpdateRequest.Email!.ToUpper();
                    appUser.NormalizedUserName = userUpdateRequest.Email.ToUpper();
                }

                var result = await userManager.UpdateAsync(appUser);

                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return Results.BadRequest($"Failed to update user: {errors}");
                }

                return Results.Ok("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 UpdateUserData error: " + ex);
                return Results.Problem("Internal server error: " + ex.Message);
            }
        }


        public static async Task<IResult> GetUserOrderHistory(ClaimsPrincipal userPrincipal,
                                                              UserManager<ApplicationUser> userManager,
                                                              IOrderService orderService)
        {
            ApplicationUser? user = await userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                return Results.Problem("Error while getting active user!");
            }

            List<OrderResponse> userOrderHistory = await orderService.GetUserOrderHistory(user.Id);

            return Results.Ok(userOrderHistory);
        }
    }
}
