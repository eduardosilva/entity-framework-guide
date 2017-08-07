using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core
{
    public class Name
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName()
        {
            return String.Format("{0} {1}, {2} {3}", Title, LastName, FirstName, MiddleName);
        }

        public override string ToString()
        {
            return FullName();
        }
    }
}
