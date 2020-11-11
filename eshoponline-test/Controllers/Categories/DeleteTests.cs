using eshoponline.Controllers.Categories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Categories
{
    public class DeleteTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Delete_Category()
        {
            await MainHelper.CreateRandomData(this);

            //Create category
            var commandCreate = new Create.Command()
            {
                Name = "Books",
                Description = "Description book",
                Slug = "books"
            };
            var category = await CategoryHelper.CreateCategory(this, commandCreate);

            //Delete category
            var commandDelete = new Delete.Command()
            {
                CategoryId = category.CategoryId
            };

            var dbContext = this.GetDbContext();
            var categoryDeleteHandler = new Delete.Handler(dbContext);
            await categoryDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test category existence
            var dbCategory = await ExecuteDbContextAsync(db => db.Categories.Where(d => d.CategoryId == category.CategoryId).SingleOrDefaultAsync());

            Assert.Null(dbCategory);
        }

        [Fact]
        public async Task Expect_Delete_Category_with_product_1()
        {
            await MainHelper.CreateRandomData(this);

            //Create category
            var commandCreate = new Create.Command()
            {
                Name = "Books",
                Description = "Description book",
                Slug = "books"
            };
            var category = await CategoryHelper.CreateCategory(this, commandCreate);

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

            //Delete category
            var commandDelete = new Delete.Command()
            {
                CategoryId = category.CategoryId
            };

            var dbContext = this.GetDbContext();
            var categoryDeleteHandler = new Delete.Handler(dbContext);
            await categoryDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test product main category is null
            var dbProduct = await ExecuteDbContextAsync(db => db.Products.Where(p => p.ProductId == product.ProductId).SingleOrDefaultAsync());

            Assert.Null(dbProduct.MainCategoryId);
        }

        [Fact]
        public async Task Expect_Delete_Category_with_product_2()
        {
            //Create category
            var commandCreate = new Create.Command()
            {
                Name = "Books",
                Description = "Description book",
                Slug = "books"
            };
            var category = await CategoryHelper.CreateCategory(this, commandCreate);

            //Create product
            var commandCreateProduct = new eshoponline.Controllers.Products.Create.Command()
            {
                Name = "Book 1",
                Description = "Description book",
                Slug = "book_1",
                Summary = "Summary",
                CategoriesIds = new int[] { category.CategoryId }
            };
            var product = await eshoponline_test.Controllers.Products.ProductHelper.CreateProduct(this, commandCreateProduct);

            //Test categories list have one item
            Assert.Equal(1, product.Categories.Count);

            //Delete category
            var commandDelete = new Delete.Command()
            {
                CategoryId = category.CategoryId
            };

            var dbContext = this.GetDbContext();
            var categoryDeleteHandler = new Delete.Handler(dbContext);
            await categoryDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test categories list empty
            var dbProduct = await ExecuteDbContextAsync(db => db.Products.Where(p => p.ProductId == product.ProductId).SingleOrDefaultAsync());
            Assert.Equal(0, dbProduct.Categories.Count);
        }
    }
}
