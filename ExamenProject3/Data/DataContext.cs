using ExamenProject3.Models.Order;
using ExamenProject3.Models.Product;
using ExamenProject3.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ExamenProject3.Data
{
    public class DataContext: DbContext
    {
        public DataContext()
        {
                
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<ProductCategoryEntity> Categories { get; set; } = null!;
        public virtual DbSet<ProductEntity> Products { get; set; } = null!;

        public virtual DbSet<OrderEntity> Orders { get; set; } = null!;
        public virtual DbSet<OrderRowEntity> OrderRows { get; set; } = null!;
        public virtual DbSet<UserEntity> Users { get; set; } = null!;

        public virtual DbSet<UserAddressEntity> Addresses { get; set; } = null!;





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderRowEntity>()
                .HasKey(c => new { c.OrderId, c.ArticaleNumber });
        }
    }
}
