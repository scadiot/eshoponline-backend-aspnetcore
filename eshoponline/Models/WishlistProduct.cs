using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Models
{
    public class WishlistProduct
    {
        public int WishlistProductId { get; set; }

        // n-1 relationships
        public int UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
