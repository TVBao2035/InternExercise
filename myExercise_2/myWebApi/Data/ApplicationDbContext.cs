using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using myWebApi.Enity;

namespace myWebApi.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .ToTable("Order").HasKey(s => s.Id);
            modelBuilder.Entity<Order>()
                .HasOne(s => s.User);

            modelBuilder.Entity<Order>().HasMany(o => o.OrderDetails).WithOne(od => od.Order);
                
        }
    }
}
