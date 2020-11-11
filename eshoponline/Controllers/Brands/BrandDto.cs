using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Brands
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int ProductsCount { get; set; }
    }
}
