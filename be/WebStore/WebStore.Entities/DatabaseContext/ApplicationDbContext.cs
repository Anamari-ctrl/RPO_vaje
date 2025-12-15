using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public virtual DbSet<Review>? Reviews { get; set; }
        public virtual DbSet<Rating>? Ratings { get; set; }
        public virtual DbSet<ShoppingCart>? ShoppingCarts { get; set; }
        public virtual DbSet<ShoppingCartItem>? ShoppingCartItem { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Branch>().ToTable("Branches");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.Property(o => o.TotalAmount)
                      .HasPrecision(18, 2);

                entity.Property(x => x.OrderNumber)
                      .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");

                entity.Property(oi => oi.PriceAtPurchase)
                      .HasPrecision(18, 2);

                entity.HasOne(orderItem => orderItem.Order)
                      .WithMany(order => order.OrderItems)
                      .HasForeignKey(orderItem => orderItem.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(orderItem => orderItem.Product)
                      .WithMany()
                      .HasForeignKey(orderItem => orderItem.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Rating>().ToTable("Rating");

            modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCarts");
            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.ToTable("ShoppingCartItems");

                entity.HasOne(x => x.ShoppingCart)
                      .WithMany(x => x.Items)
                      .HasForeignKey(x => x.CartId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(shoppingCartItem => shoppingCartItem.Product)
                      .WithMany()
                      .HasForeignKey(shoppingCartItem => shoppingCartItem.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
