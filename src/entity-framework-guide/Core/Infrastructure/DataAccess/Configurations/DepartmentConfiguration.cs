using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            ToTable(tableName: "Department", schemaName: "HumanResources");

            //Property(t => t.Id).HasColumnName("DepartmentID");
            Property(t => t.Name).IsRequired();
            Property(t => t.GroupName).IsRequired();
        }
    }
}
