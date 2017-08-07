using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Conventions
{
    public class EntityConvention : Convention
    {
        public EntityConvention()
        {
            Types().Where(t => t.IsAbstract == false && 
                               (
                                    (t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(Entity<>)) ||
                                    t.BaseType == typeof(Entity)
                                )
                          )
                   .Configure(t =>
                   {
                       t.Property("Id").IsKey().HasColumnName(t.ClrType.Name + "ID");
                   });
        }
    }
}
