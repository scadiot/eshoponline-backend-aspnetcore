using eshoponline.Controllers.Products;
using eshoponline_test.Controllers.Categories;
using eshoponline_test.Controllers.CompareGroups;
using eshoponline_test.Controllers.Specifications;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Products
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Product()
        {
            await MainHelper.CreateRandomData(this);

            var command = new Create.Command()
            {
                Name = "Book",
                Description = "Description book",
                Slug = "book",
                Summary = "Summary book",
                UnitPrice = 1.22m,
                UnitsInStock = 5,
                Features = new string[] { "feature 1", "feature 2" },
                Keywords = new string[] { "word 1", "word 2", "word 3" },
            };

            var productCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var product = await productCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(product);

            Assert.Equal(command.Name, product.Name);
            Assert.Equal(command.Description, product.Description);
            Assert.Equal(command.Slug, product.Slug);
            Assert.Equal(command.Summary, product.Summary);
            Assert.Equal(command.UnitPrice, product.UnitPrice);
            Assert.Equal(command.UnitsInStock, product.UnitsInStock);
            Assert.Equal(command.Features.Length, product.Features.Length);
        }

        [Fact]
        public async Task Expect_Create_Product_with_categories()
        {
            var commandCreateCategory = new eshoponline.Controllers.Categories.Create.Command()
            {
                Name = "Books",
                Description = "Description book",
                Slug = "books"
            };
            var category = await CategoryHelper.CreateCategory(this, commandCreateCategory);

            var command = new Create.Command()
            {
                Name = "Book",
                Description = "Description book",
                Slug = "book",
                Summary = "Summary book",
                UnitPrice = 1.22m,
                UnitsInStock = 5,
                CategoriesIds = new int[] { category.CategoryId }
            };

            var productCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var product = await productCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.Single(product.CategoryIds);
            Assert.Equal(category.CategoryId, product.CategoryIds[0]);
        }

        [Fact]
        public async Task Expect_Create_Product_with_specifications()
        {
            var commandCreateSpecification1 = new eshoponline.Controllers.Specifications.Create.Command()
            {
                Name = "Frequency",
                LongName = "Processor frequency",
                Type = eshoponline.Models.SpecificationType.Decimal,
                Unity = "Ghz"
            };
            var specification1 = await SpecificationHelper.CreateSpecification(this, commandCreateSpecification1);

            var commandCreateSpecification2 = new eshoponline.Controllers.Specifications.Create.Command()
            {
                Name = "Cache",
                LongName = "Cache processor",
                Type = eshoponline.Models.SpecificationType.Interger,
                Unity = "Mo"
            };
            var specification2 = await SpecificationHelper.CreateSpecification(this, commandCreateSpecification2);

            var command = new Create.Command()
            {
                Name = "Book",
                Description = "Description book",
                Slug = "book",
                Summary = "Summary book",
                UnitPrice = 1.22m,
                UnitsInStock = 5,
                ProductSepcifications = new Create.ProductSepcification[]
                {
                    new Create.ProductSepcification() { DecimalValue = 1.2M, SpecificationId = specification1.SpecificationId },
                    new Create.ProductSepcification() { IntegerValue = 16, SpecificationId = specification2.SpecificationId }
                }
            };

            var productCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var product = await productCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.Equal(command.ProductSepcifications.Length, command.ProductSepcifications.Length);
        }

        [Fact]
        public async Task Expect_Create_Product_with_CompareGroup()
        {
            var commandCreateCompareGroup = new eshoponline.Controllers.CompareGroups.Create.Command()
            {
                Name = "Computer",
            };
            var compareGroup = await CompareGroupHelper.CreateCompareGroup(this, commandCreateCompareGroup);

            var command = new Create.Command()
            {
                Name = "Ideapad",
                Description = "Good computer",
                Slug = "ideapad",
                Summary = "Good computer",
                UnitPrice = 1.22m,
                UnitsInStock = 5,
                CompareGroupId = compareGroup.CompareGroupId
            };

            var productCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var product = await productCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.Equal(compareGroup.CompareGroupId, product.CompareGroupId);
        }
    }
}
