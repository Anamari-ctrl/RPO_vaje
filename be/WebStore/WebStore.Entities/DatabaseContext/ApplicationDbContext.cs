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

            GetMockData<OrderItem>("orderitems", modelBuilder);

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
    }
}
