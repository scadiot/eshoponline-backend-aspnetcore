using eshoponline.Controllers.Categories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Categories
{
    public class ListTests : SliceFixture
    {
        [Fact]
        public async Task Expect_List_Categories()
        {
            await MainHelper.CreateRandomData(this);

            //Create categories
            var commandCreate1 = new Create.Command()
            {
                Name = "Livre 1",
                Description = "Description Livre",
                Slug = "livre_1"
            };
            var category1 = await CategoryHelper.CreateCategory(this, commandCreate1);

            var commandCreate2 = new Create.Command()
            {
                Name = "Livre 2",
                Description = "Description Livre",
                Slug = "livre_2"
            };
            var category2 = await CategoryHelper.CreateCategory(this, commandCreate2);

            var commandCreate3 = new Create.Command()
            {
                Name = "Livre 3",
                Description = "Description Livre",
                Slug = "livre_3"
            };
            var category3 = await CategoryHelper.CreateCategory(this, commandCreate3);

            //List categories
            var command = new List.Query();
            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();
            var categoriesListHandler = new List.Handler(dbContext, mapper);
            var categoriesList = await categoriesListHandler.Handle(command, new System.Threading.CancellationToken());

            //List categories
            var dbCategories = await ExecuteDbContextAsync(db => db.Categories.ToListAsync());

            Assert.Equal(dbCategories.Count, categoriesList.CategoriesCount);
        }
    }
}
