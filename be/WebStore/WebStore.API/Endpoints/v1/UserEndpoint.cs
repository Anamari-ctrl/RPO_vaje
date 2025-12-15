using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebStore.Entities.Identity;
using WebStore.ServiceContracts;
using WebStore.ServiceContracts.DTO.Account;
using WebStore.ServiceContracts.DTO.OrderDTO;

namespace WebStore.API.Endpoints.v1
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/users/me", GetUserData).RequireAuthorization();
            app.MapPut("api/v1/users/me", UpdateUserData).RequireAuthorization();
            app.MapPut("api/v1/users/orderHistory", GetUserOrderHistory).RequireAuthorization();
        }

        public static async Task<IResult> GetUserData(string? userId,
                                                      UserManager<ApplicationUser> userManager,
                                                      ClaimsPrincipal userPrincipal)
        {
            ApplicationUser? user = null;

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
                                                         UserManager<ApplicationUser> userManager)
        {
            if (userUpdateRequest == null)
            {
                return Results.Problem("Provided user was null!");
            }

            ApplicationUser? appUser = await userManager.FindByIdAsync(userUpdateRequest.UserId.ToString());

            if (appUser == null)
            {
                return Results.Problem("User was not found!");
            }

            appUser.FirstName = userUpdateRequest.FirstName;
            appUser.LastName = userUpdateRequest.LastName;
            appUser.Email = userUpdateRequest.Email;
            appUser.Address = userUpdateRequest.Address;
            appUser.City = userUpdateRequest.City;
            appUser.Country = userUpdateRequest.Country;
            appUser.PostalCode = userUpdateRequest.PostalCode;
            appUser.PhoneNumber = userUpdateRequest.PhoneNumber;

            IdentityResult result = await userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
            {
                return Results.Problem("Error while changing user data!");
            }

            return Results.Ok(userUpdateRequest);
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
