using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Configurations
{
    public class PersonPasswordConfiguration : EntityTypeConfiguration<PersonPassword>
    {
        public PersonPasswordConfiguration()
        {
            ToTable("Password", "Person");

            Property(t => t.Hash).HasColumnName("PasswordHash");
            Property(t => t.Salt).HasColumnName("PasswordSalt");
            Property(t => t.RowId).HasColumnName("rowguid");
        }
    }
}
