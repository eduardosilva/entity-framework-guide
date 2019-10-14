using entity_framework_guide.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class PhotoConfiguration : EntityTypeConfiguration<Photo>
    {
        public PhotoConfiguration()
        {
            ToTable("ProductPhoto", "Production");
            Property(t => t.Id).HasColumnName("ProductPhotoId");

            Property(t => t.Thumbnail).HasColumnName("ThumbnailPhoto");
            Property(t => t.ThumbnailName).HasColumnName("ThumbnailPhotoFileName");
            Property(t => t.Large).HasColumnName("LargePhoto");
            Property(t => t.LargeName).HasColumnName("LargePhotoFileName");
        }
    }
}
