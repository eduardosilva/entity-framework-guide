using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.Conventions
{
    public class MakeAllStringsNonUnicodeConvention : Convention
    {
        public MakeAllStringsNonUnicodeConvention()
        {
            this.Properties<string>().Configure(t => t.IsUnicode(false));
        }
    }
}
