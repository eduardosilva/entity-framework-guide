using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities
{
    public class PersonPassword : Entity
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
        public Guid RowId { get; set; }
    }
}
