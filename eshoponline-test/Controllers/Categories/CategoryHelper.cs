using eshoponline.Controllers.Categories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.Categories
{
    class CategoryHelper
    {
        public static async Task<eshoponline.Models.Category> CreateCategory(SliceFixture fixture, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var categoryCreateHandler = new Create.Handler(dbContext, mapper);
            var created = await categoryCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbCategory = await fixture.ExecuteDbContextAsync(db => db.Categories.Where(a => a.CategoryId == created.CategoryId)
                .SingleOrDefaultAsync());

            return dbCategory;
        }

        public static async Task<eshoponline.Models.Category> CreateRandomCategory(SliceFixture fixture, int? parentCategory = null)
        {
            var command = new Create.Command()
            {
                Name = "Random Cathegory",
                Description = "Random cathegory description",
                Slug = "random_cathegory",
                ParentCategoryId = parentCategory
            };
            return await CategoryHelper.CreateCategory(fixture, command);
        }

        public static async Task<eshoponline.Models.Category> GetCategory(SliceFixture fixture, int categoryId)
        {
            var dbCategory = await fixture.ExecuteDbContextAsync(db => db.Categories.Where(a => a.CategoryId == categoryId)
                .SingleOrDefaultAsync());

            return dbCategory;
        }
    }
}
