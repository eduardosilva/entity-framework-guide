using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            ToTable("Employee", "HumanResources");

            Property(t => t.NationalIDNumber).HasMaxLength(15).IsRequired();
            HasMany(t => t.JobCandidates).WithRequired().Map(t => t.MapKey("BusinessEntityID"));
        }
    }
}
