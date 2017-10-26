using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity_framework_guide.Core.Entities;

namespace entity_framework_guide.Core.Infrastructure.DataAccess.ChangeTrackers
{
    public class AudityChangeTracker : ChangeTracker<Auditable>
    {
        public override void Added(Auditable entity)
        {
            entity.ModifiedDate = DateTime.Now;
        }

        public override void Deleted(Auditable entity)
        {

        }

        public override void Updated(Auditable entity)
        {
            entity.ModifiedDate = DateTime.Now;
        }
    }
}
