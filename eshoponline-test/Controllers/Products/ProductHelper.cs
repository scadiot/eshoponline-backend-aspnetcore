using eshoponline.Controllers.Products;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.Products
{
    class ProductHelper
    {
        public static async Task<eshoponline.Models.Product> CreateProduct(SliceFixture fixture, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var categoryCreateHandler = new Create.Handler(dbContext, mapper);
            var created = await categoryCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbProduct = await fixture.ExecuteDbContextAsync(
                db => db.Products
                .Where(a => a.ProductId == created.ProductId)
                .Include(a => a.Categories)
                .Include(a => a.Features)
                .SingleOrDefaultAsync());

            return dbProduct;
        }

        public static async Task<eshoponline.Models.Product> CreateRandomProduct(SliceFixture fixture, int? mainCategoryId = null, int[] categoriesIds = null)
        {
            var commandCreateProduct = new eshoponline.Controllers.Products.Create.Command()
            {
                Name = "Book 1",
                Description = "Description book",
                Slug = "book_1",
                Summary = "Summary",
                MainCategoryId = mainCategoryId,
                CategoriesIds = categoriesIds,
                UnitPrice = 10,
                Keywords = new string[] { "Keyword 1", "Keyword 2" },
                Features = new string[] { "Feature 1", "Feature 2" }
            };
            return await eshoponline_test.Controllers.Products.ProductHelper.CreateProduct(fixture, commandCreateProduct);
        }

        public static async Task DeleteProduct(SliceFixture fixture, eshoponline.Controllers.Products.Delete.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var productDeleteHandler = new Delete.Handler(dbContext);
            await productDeleteHandler.Handle(command, new System.Threading.CancellationToken());
        }
    }
}
