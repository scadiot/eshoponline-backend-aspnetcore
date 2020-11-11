namespace eshoponline.Controllers.Categories
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public int? ParentCategoryId { get; set; }
        public int ItemsCount { get; set; }
    }
}
