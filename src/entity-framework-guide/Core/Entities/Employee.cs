using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities
{
    public class Employee : Person
    {
        public Employee()
        {
            HistoryDepartments = new List<EmployeeDepartment>();
            JobCandidates = new List<JobCandidate>();
        }

        public string NationalIDNumber { get; set; }
        public virtual ICollection<EmployeeDepartment> HistoryDepartments { get; set; }
        public ICollection<JobCandidate> JobCandidates { get; set; }
    }
}
