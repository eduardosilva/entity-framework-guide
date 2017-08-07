using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    internal class EmployeeDepartmentConfiguration : EntityTypeConfiguration<EmployeeDepartment>
    {
        public EmployeeDepartmentConfiguration()
        {
            ToTable("EmployeeDepartmentHistory", "HumanResources");
            HasKey(t => new { t.BusinessEntityId, t.DepartmentId, t.ShiftId, t.StartDate });

            Property(t => t.BusinessEntityId).HasColumnName("BusinessEntityID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(t => t.DepartmentId).HasColumnName("DepartmentID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(t => t.ShiftId).HasColumnName("ShiftID").IsRequired();
            Property(t => t.StartDate).HasColumnName("StartDate").IsRequired();
            Property(t => t.EndDate).HasColumnName("EndDate").IsOptional();
            Property(t => t.ModifiedDate).HasColumnName("ModifiedDate").IsRequired();


            HasRequired(t => t.Employee).WithMany(t => t.HistoryDepartments).HasForeignKey(t => t.BusinessEntityId);
            HasRequired(t => t.Department).WithMany().HasForeignKey(t => t.DepartmentId);
            HasRequired(t => t.Shift).WithMany().HasForeignKey(t => t.ShiftId);
        }
    }
}
