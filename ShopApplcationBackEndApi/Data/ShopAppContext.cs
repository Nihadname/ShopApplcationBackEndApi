using Microsoft.EntityFrameworkCore;
using ShopApplcationBackEndApi.Entities;
using System.Reflection;

namespace ShopApplcationBackEndApi.Data
{
    public class ShopAppContext : DbContext
    {
        public ShopAppContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
