using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Conventions
{
    public class NameFieldConvention : Convention
    {
        public NameFieldConvention()
        {
            Properties<string>().Where(t => t.Name.Contains("Name"))
                                .Configure(t => t.HasMaxLength(50));
        }
    }
}
