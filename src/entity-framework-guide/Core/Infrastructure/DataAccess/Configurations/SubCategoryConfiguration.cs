using entity_framework_guide.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class SubCategoryConfiguration : EntityTypeConfiguration<SubCategory>
    {
        public SubCategoryConfiguration()
        {
            ToTable("ProductSubcategory", "Production");

            Property(t => t.Id).HasColumnName("ProductSubCategoryId");
        }
    }
}
