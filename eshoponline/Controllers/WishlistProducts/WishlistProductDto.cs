using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Controllers.WishlistProducts
{
    public class WishlistProductDto
    {
        public int WishlistProductId { get; set; }
        public int UserId { get; set; }
        public Products.ProductDto Product { get; set; }
    }
}
