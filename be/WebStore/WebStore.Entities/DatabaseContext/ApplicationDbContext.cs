using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebStore.Entities.Identity;
using WebStore.Entities.Models;

namespace WebStore.Entities.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Branch>? Branches { get; set; }
        public virtual DbSet<Category>? Categories { get; set; }
        public virtual DbSet<Order>? Orders { get; set; }
        public virtual DbSet<OrderItem>? OrderItem { get; set; }
        public virtual DbSet<Product>? Products { get; set; }
        public virtual DbSet<Rating>? Ratings { get; set; }
        public virtual DbSet<Genre>? Genres { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CreateUsers(modelBuilder);

            modelBuilder.Entity<Branch>().ToTable("Branches");

            GetMockData<Branch>("branches", modelBuilder);

            modelBuilder.Entity<Category>().ToTable("Categories");

            GetMockData<Category>("categories", modelBuilder);

            modelBuilder.Entity<Genre>().ToTable("Genres");

            GetMockData<Genre>("genres", modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasMany(x => x.Ratings)
                      .WithOne()
                      .HasForeignKey(x => x.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            GetMockData<Product>("products", modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.Property(o => o.TotalAmount)
                      .HasPrecision(18, 2);

                entity.Property(x => x.OrderNumber)
                      .ValueGeneratedOnAdd();

                entity.HasMany(x => x.OrderItems)
                      .WithOne(x => x.Order)
                      .HasForeignKey(x => x.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            GetMockData<Order>("orders", modelBuilder);

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");

                entity.Property(oi => oi.PriceAtPurchase)
                      .HasPrecision(18, 2);

                entity.HasOne(orderItem => orderItem.Product)
                      .WithMany()
                      .HasForeignKey(orderItem => orderItem.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            GetMockData<OrderItem>("orderItems", modelBuilder);

            modelBuilder.Entity<Rating>().ToTable("Rating");

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(x => x.RatingId);

                entity.HasIndex(x => x.ProductId);
            });
        }

        private static void GetMockData<T>(string fileName,
                                           ModelBuilder modelBuilder) where T : class
        {
            string? mockJsonData = File.ReadAllText($"MockData/Mock_{fileName}.json");

            List<T>? items = JsonSerializer.Deserialize<List<T>>(mockJsonData);

            if (items != null)
            {
                foreach (var item in items)
                {
                    modelBuilder.Entity<T>().HasData(item);
                }
            }
        }

        private static void CreateUsers(ModelBuilder modelBuilder)
        {
            List<ApplicationUser> users = [];

            var user = new ApplicationUser
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000001"),
                UserName = "admin@webstore.com",
                NormalizedUserName = "ADMIN@WEBSTORE.COM",
                Email = "admin@webstore.com",
                NormalizedEmail = "ADMIN@WEBSTORE.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                FirstName = "Admin",
                LastName = "User"
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "SecurePassword123!");

            users.Add(user);

            user = new ApplicationUser()
            {
                Id = Guid.Parse("61000000-0000-0000-0000-000000000001"),
                UserName = "miha@webstore.com",
                NormalizedUserName = "MIHA@WEBSTORE.COM",
                Email = "miha@webstore.com",
                NormalizedEmail = "MIHA@WEBSTORE.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                FirstName = "Miha",
                LastName = "Potočnik"
            };

            passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "SecurePassword123!");

            users.Add(user);

            user = new ApplicationUser()
            {
                Id = Guid.Parse("62000000-0000-0000-0000-000000000001"),
                UserName = "anamari@webstore.com",
                NormalizedUserName = "ANAMARI@WEBSTORE.COM",
                Email = "anamari@webstore.com",
                NormalizedEmail = "ANAMARI@WEBSTORE.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                FirstName = "Anamari",
                LastName = "Orehar"
            };

            passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "SecurePassword123!");

            users.Add(user);

            user = new ApplicationUser()
            {
                Id = Guid.Parse("63000000-0000-0000-0000-000000000001"),
                UserName = "misha@webstore.com",
                NormalizedUserName = "MISHA@WEBSTORE.COM",
                Email = "misha@webstore.com",
                NormalizedEmail = "MISHA@WEBSTORE.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                FirstName = "Misha",
                LastName = "Rozman Atelšek"
            };

            passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "SecurePassword123!");

            users.Add(user);

            modelBuilder.Entity<ApplicationUser>().HasData(users);

            var adminRoleId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );

            List<IdentityUserRole<Guid>> userRoles = [
                  new IdentityUserRole<Guid>
                {
                    RoleId = adminRoleId,
                    UserId = Guid.Parse("60000000-0000-0000-0000-000000000001")
                },
                  new IdentityUserRole<Guid>
                {
                    RoleId = adminRoleId,
                    UserId = Guid.Parse("61000000-0000-0000-0000-000000000001")
                },
                    new IdentityUserRole<Guid>
                {
                    RoleId = adminRoleId,
                    UserId = Guid.Parse("62000000-0000-0000-0000-000000000001")
                },
                  new IdentityUserRole<Guid>
                {
                    RoleId = adminRoleId,
                    UserId = Guid.Parse("63000000-0000-0000-0000-000000000001")
                }
                ];

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(userRoles);
        }
    }
}
