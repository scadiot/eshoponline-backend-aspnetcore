
using eshoponline.Controllers.Categories;

namespace eshoponline.Controllers.Products
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int ImagesCount { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public decimal Star { get; set; }
        public int? MainCategoryId { get; set; }
        public int? CompareGroupId { get; set; }

        public int[] CategoryIds { get; set; }
        public string[] Keywords { get; set; }
        public string[] Features { get; set; }
    }
}
