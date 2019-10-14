using entity_framework_guide.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class DatabaseLogConfiguration : EntityTypeConfiguration<DatabaseLog>
    {
        public DatabaseLogConfiguration()
        {
            ToTable("DatabaseLog");

            Property(t => t.Id).HasColumnName("DatabaseLogID");
        }
    }
}
