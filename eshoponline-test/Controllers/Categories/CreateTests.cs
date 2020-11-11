using eshoponline.Controllers.Categories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Categories
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Categories()
        {
            await MainHelper.CreateRandomData(this);

            var command = new Create.Command()
            {
                Name = "Books",
                Description = "Description book",
                Slug = "books"
            };

            var categoryCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var category = await categoryCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(category);
            Assert.Equal(command.Name, category.Name);
            Assert.Equal(command.Slug, category.Slug);
            Assert.Equal(command.Description, category.Description);
        }

        [Fact]
        public async Task Expect_Create_Categories_with_parent()
        {
            await MainHelper.CreateRandomData(this);

            var command1 = new Create.Command()
            {
                Name = "Books",
                Description = "Description book",
                Slug = "books"
            };
            var categoryParent = await CategoryHelper.CreateCategory(this, command1);

            var command2 = new Create.Command()
            {
                Name = "Best sellers",
                Description = "Description Best sellers",
                Slug = "best_sellers",
                ParentCategoryId = categoryParent.CategoryId
            };
            var category = await CategoryHelper.CreateCategory(this, command2);

            Assert.NotNull(category.Parent);
            Assert.Equal(command1.Name, category.Parent.Name);
        }
    }
}
