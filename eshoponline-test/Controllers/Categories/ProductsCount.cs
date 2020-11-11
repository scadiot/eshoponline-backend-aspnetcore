using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Categories
{
    public class ProductsCount : SliceFixture
    {
        [Fact]
        public async Task Expect_products_count_add()
        {
            await MainHelper.CreateRandomData(this);

            var commandCreateCategory = new eshoponline.Controllers.Categories.Create.Command()
            {
                Name = "Books",
                Description = "Description books",
                Slug = "books"
            };
            var category = await CategoryHelper.CreateCategory(this, commandCreateCategory);

            var commandCreateProduct = new eshoponline.Controllers.Products.Create.Command()
            {
                Name = "Book 1",
                Description = "Description book",
                Slug = "book_1",
                MainCategoryId = category.CategoryId,
                Summary = "Summary",
            };
            var product = await eshoponline_test.Controllers.Products.ProductHelper.CreateProduct(this, commandCreateProduct);
            category = await CategoryHelper.GetCategory(this, category.CategoryId);

            Assert.Equal(1, category.ProductsCount);
        }

        [Fact]
        public async Task Expect_products_count_add_remove()
        {
            await MainHelper.CreateRandomData(this);

            //Create category
            var commandCreateCategory = new eshoponline.Controllers.Categories.Create.Command()
            {
                Name = "Books",
                Description = "Description books",
                Slug = "books"
            };
            var category = await CategoryHelper.CreateCategory(this, commandCreateCategory);

            //Create product
            var commandCreateProduct = new eshoponline.Controllers.Products.Create.Command()
            {
                Name = "Book 1",
                Description = "Description book",
                Slug = "book_1",
                MainCategoryId = category.CategoryId,
                Summary = "Summary",
            };
            var product = await eshoponline_test.Controllers.Products.ProductHelper.CreateProduct(this, commandCreateProduct);

            //Verify product count
            category = await CategoryHelper.GetCategory(this, category.CategoryId);
            Assert.Equal(1, category.ProductsCount);

            //Delete product
            var commandDeleteProduct = new eshoponline.Controllers.Products.Delete.Command()
            {
                ProductId = product.ProductId
            };
            await eshoponline_test.Controllers.Products.ProductHelper.DeleteProduct(this, commandDeleteProduct);

            //Verify product count
            category = await CategoryHelper.GetCategory(this, category.CategoryId);
            Assert.Equal(0, category.ProductsCount);
        }
    }
}
