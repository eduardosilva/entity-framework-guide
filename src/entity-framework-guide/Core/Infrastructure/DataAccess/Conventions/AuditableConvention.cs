using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Conventions
{
    public class AuditableConvention : Convention
    {
        public AuditableConvention()
        {
            Types().Where(t => typeof(Auditable).IsAssignableFrom(t))
                   .Configure(t =>
                   {
                       t.Property("ModifiedDate").IsRequired();
                   });
        }
    }
}
