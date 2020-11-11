using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CartProducts
{
    public class CartProductDto
    {
        public int CartProductId { get; set; }
        public int UserId { get; set; }
        public Products.ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
