using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
        public int Number { get; set; }
        public int? Severity { get; set; }
        public int? State { get; set; }
        public string Procedure { get; set; }
        public int Line { get; set; }
        public string Message { get; set; }
    }
}
