using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopApplcationBackEndApi.Entities;

namespace ShopApplcationBackEndApi.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
           builder.Property(s=>s.Name).IsRequired(true).HasMaxLength(80);
            builder.Property(s => s.SalePrice).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(s => s.CostPrice).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(s=>s.IsDeleted).HasDefaultValue(false);
            builder.Property(s => s.CreatedTime).HasDefaultValueSql("GETDATE()");
            builder.Property(s => s.UpdatedTime).HasDefaultValueSql("GETDATE()");


        }
    }
}
