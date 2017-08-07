using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Conventions
{
    public class PersonConvention : Convention
    {
        public PersonConvention()
        {
            var personType = typeof(Person);

            Types().Where(t => t == personType || t.BaseType == personType)
                   .Configure(t =>
                   {
                       t.Property("Id").IsKey().HasColumnName("BusinessEntityID");
                   });
        }
    }
}
