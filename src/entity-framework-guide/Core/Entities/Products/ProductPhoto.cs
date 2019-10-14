using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities.Products
{
    public class ProductPhoto
    {
        public int ProductId { get; set; }
        public int PhotoId { get; set; }
        public bool Primary { get; set; }

        public virtual Photo Photo { get; set; }
    }
}
