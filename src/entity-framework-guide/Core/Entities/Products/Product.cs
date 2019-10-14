using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities.Products
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public SubCategory SubCategory { get; set; }
        public ICollection<ProductPhoto> Photos { get; set; }
    }
}
