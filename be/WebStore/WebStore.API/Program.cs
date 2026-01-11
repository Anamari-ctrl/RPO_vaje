using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebStore.API.Endpoints.v1;
using WebStore.Entities.DatabaseContext;
using WebStore.Entities.Identity;
using WebStore.Repositories;
using WebStore.RepositoryContracts;
using WebStore.ServiceContracts;
using WebStore.Services;

namespace WebStore.API
{
    public class Program
    {
        public void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Repositories
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IBranchRepository, BranchRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();

            //Services
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<IBranchService, BranchService>();
            builder.Services.AddTransient<IRatingService, RatingService>();

            // Add services to the container.

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")!);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

                    if (allowedOrigins != null)
                    {
                        policyBuilder.WithOrigins(allowedOrigins);
                        policyBuilder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
                        //.WithHeaders("Authorization", "origin", "accept", "content-type");
                    }
                });
            });

            //Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                //options.Password.RequiredLength = 10;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleManager<RoleManager<ApplicationRole>>();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(2);
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine("JWT Token validation failed!");
                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //Endpoints
            app.MapAuthEndpoints();
            app.MapBooksEndpoints();
            app.MapBranchEndpoints();
            app.MapOrderEndpoints();
            app.MapProductEndpoints();
            app.MapRatingEndpoints();
            app.MapUserEndpoints();

            app.UseCors();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        await TestUserGenerator.CreateUserAsync(services);
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred while seeding the database.");
            //    }
            //}

            app.Run();
        }
    }
}
