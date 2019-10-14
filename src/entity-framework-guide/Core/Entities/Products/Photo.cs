using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Entities.Products
{
    public class Photo : Entity
    {
        public byte[] Thumbnail { get; set; }
        public string ThumbnailName { get; set; }
        public byte[] Large { get; set; }
        public string LargeName { get; set; }
    }
}
