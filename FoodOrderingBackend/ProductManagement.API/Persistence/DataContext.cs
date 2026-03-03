using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Entities;

namespace ProductManagement.API.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<WeightType> WeightTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductVariant>()   
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
