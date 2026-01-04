using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Entities.Identity;

namespace WebStore.Services.Helpers
{
    public static class TestUserGenerator
    {
        public static async Task CreateUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            string userRole = "User";

            if (!await roleManager.RoleExistsAsync(userRole))
            {
                await roleManager.CreateAsync(new ApplicationRole(userRole));
            }

            var testUserEmail = "test@example.com";
            var testUser = await userManager.FindByEmailAsync(testUserEmail);

            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = testUserEmail,
                    Email = testUserEmail,
                    EmailConfirmed = true,
                    FirstName = "Test",
                    LastName = "Tester",
                    Address = "Testna cesta 1",
                    City = "Novo Mesto",
                    PostalCode = "8000",
                    Country = "Slovenija",
                };

                var result = await userManager.CreateAsync(testUser, "Password1234!");

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    Console.Error.WriteLine($"Error creating user: {errors}");
                }
            }
        }

        public static async Task CreateAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            string adminRole = "Admin";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new ApplicationRole(adminRole));
            }

            var userEmail = "admin@mail.com";
            var user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Administrator",
                    Address = "Ljubljanska cesta 31a",
                    City = "Novo Mesto",
                    PostalCode = "8000",
                    Country = "Slovenija",
                };

                var result = await userManager.CreateAsync(user, "Password123!");

                if (!result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);

                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    Console.Error.WriteLine($"Error creating user: {errors}");
                }
            }
        }
    }
}
