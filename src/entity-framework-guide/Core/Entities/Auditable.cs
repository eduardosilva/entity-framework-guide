using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities
{
    public abstract class Auditable
    {
        public DateTime ModifiedDate { get; set; }
    }
}
