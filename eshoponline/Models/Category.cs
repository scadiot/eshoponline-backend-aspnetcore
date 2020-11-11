using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eshoponline.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Slug { get; set; }

        public int? ParentCategoryId { get; set; }

        public Category Parent { get; set; }

        public int ProductsCount { get; set; }

        public virtual ICollection<ProductCategory> Products { get; set; }
    }
}
