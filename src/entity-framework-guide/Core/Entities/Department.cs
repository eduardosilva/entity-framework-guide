using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities
{
    public class Department : Entity<short>, IValidatableObject
    {
        public string Name { get; set; }
        public string GroupName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (Name == GroupName)
                result.Add(new ValidationResult("Name and group name cannot be equals"));

            return result;
        }
    }
}
