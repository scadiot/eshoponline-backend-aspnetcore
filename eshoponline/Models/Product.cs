using System;
using System.Collections.Generic;

namespace eshoponline.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public DateTime InsertDate { get; set; }

        public bool ForSale { get; set; }

        public int ImagesCount { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public double Stars { get; set; }

        public string SKU { get; set; }

        // n-1 relationships
        public int? MainCategoryId { get; set; }
        public virtual Category MainCategory { get; set; }

        public int? BrandId { get; set; }
        public virtual Brand Brand { get; set; }

        public int? CompareGroupId { get; set; }
        public virtual CompareGroup CompareGroup { get; set; }

        // 1-n relationships
        public virtual ICollection<ProductFeature> Features { get; set; }
        public virtual ICollection<ProductCategory> Categories { get; set; }
        public virtual ICollection<ProductReview> Reviews { get; set; }
        public virtual ICollection<ProductKeyword> Keywords { get; set; }
        public virtual ICollection<ProductSpecification> Specifications { get; set; }
    }
}
