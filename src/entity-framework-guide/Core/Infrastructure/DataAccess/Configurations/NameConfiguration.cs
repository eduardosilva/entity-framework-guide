using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class NameConfiguration : ComplexTypeConfiguration<Name>
    {
        public NameConfiguration()
        {
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.MiddleName).HasColumnName("MiddleName");
            Property(t => t.Title).HasColumnName("Title");
        }
    }
}
