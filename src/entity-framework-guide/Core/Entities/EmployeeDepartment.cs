using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities
{
    public class EmployeeDepartment : Auditable
    {
        //COMMENT: Is Required to composite key
        public int BusinessEntityId { get; set; }
        public short DepartmentId { get; set; }
        public byte ShiftId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Department Department { get; set; }
        public virtual Shift Shift { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
