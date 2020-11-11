using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshoponline.Models
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
