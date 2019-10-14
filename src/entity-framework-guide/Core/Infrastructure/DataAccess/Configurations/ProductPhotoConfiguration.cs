using entity_framework_guide.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class ProductPhotoConfiguration : EntityTypeConfiguration<ProductPhoto>
    {
        public ProductPhotoConfiguration()
        {
            ToTable("ProductProductPhoto", "Production");

            HasKey(t => new { t.ProductId, t.PhotoId });

            Property(t => t.PhotoId).HasColumnName("ProductPhotoId");
            Property(t => t.ProductId).HasColumnName("ProductId");

            HasRequired(t => t.Photo)
                .WithMany()
                .HasForeignKey(t => t.PhotoId);
        }
    }
}
