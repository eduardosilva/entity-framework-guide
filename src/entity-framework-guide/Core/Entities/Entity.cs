using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities
{
    public abstract class Entity : Entity<int>
    { }

    public abstract class Entity<T> : Auditable
        where T : struct
    {
        public T Id { get; set; }
    }
}
