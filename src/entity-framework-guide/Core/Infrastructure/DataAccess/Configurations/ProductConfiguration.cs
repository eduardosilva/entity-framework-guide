using entity_framework_guide.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Product", "Production");

            HasOptional(t => t.SubCategory)
                .WithMany()
                .Map(t => t.MapKey("ProductSubcategoryID"));

            HasMany(t => t.Photos)
                .WithRequired();
        }
    }
}
