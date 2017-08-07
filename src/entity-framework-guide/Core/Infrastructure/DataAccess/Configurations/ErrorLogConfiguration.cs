using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class ErrorLogConfiguration : EntityTypeConfiguration<ErrorLog>
    {
        public ErrorLogConfiguration()
        {
            this.ToTable("ErrorLog");

            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasColumnName("ErrorLogID");

            this.Property(t => t.Line)
                .HasColumnName("ErrorLine");

            this.Property(t => t.Message)
                .HasColumnName("ErrorMessage")
                .HasMaxLength(4000);

            this.Property(t => t.Number)
                .HasColumnName("ErrorNumber")
                .IsRequired();

            this.Property(t => t.Procedure)
                .HasColumnName("ErrorProcedure")
                .HasMaxLength(126);

            this.Property(t => t.Procedure)
                .HasColumnName("ErrorProcedure")
                .HasMaxLength(126);

            this.Property(t => t.Severity)
                .HasColumnName("ErrorSeverity");

            this.Property(t => t.State)
                .HasColumnName("ErrorState");

            this.Property(t => t.Time)
                .HasColumnName("ErrorTime")
                .IsRequired();

            this.Property(t => t.UserName)
                .HasColumnName("UserName")
                .HasMaxLength(128)
                .IsRequired();
        }
    }
}
