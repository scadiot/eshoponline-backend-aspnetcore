using eshoponline.Controllers.CompareGroups;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.CompareGroups
{
    public class DeleteTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Delete_CompareGroup()
        {
            await MainHelper.CreateRandomData(this);

            //Create compareGroup
            var commandCreate = new Create.Command()
            {
                Name = "Computer",
            };
            var compareGroup = await CompareGroupHelper.CreateCompareGroup(this, commandCreate);

            //Delete compareGroup
            var commandDelete = new Delete.Command()
            {
                CompareGroupId = compareGroup.CompareGroupId
            };

            var dbContext = this.GetDbContext();
            var compareGroupDeleteHandler = new Delete.Handler(dbContext);
            await compareGroupDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test compareGroup existence
            var dbCompareGroup = await ExecuteDbContextAsync(db => db.CompareGroups
                .Where(d => d.CompareGroupId == compareGroup.CompareGroupId)
                .SingleOrDefaultAsync());

            Assert.Null(dbCompareGroup);
        }

        [Fact]
        public async Task Expect_Delete_CompareGroup_with_product()
        {
            await MainHelper.CreateRandomData(this);

            //Create compareGroup
            var commandCreate = new Create.Command()
            {
                Name = "Book",
            };
            var compareGroup = await CompareGroupHelper.CreateCompareGroup(this, commandCreate);

            //Create product
            var commandCreateProduct = new eshoponline.Controllers.Products.Create.Command()
            {
                Name = "Book 1",
                Description = "Description book",
                Slug = "book_1",
                CompareGroupId = compareGroup.CompareGroupId,
                Summary = "Summary",
            };
            var product = await eshoponline_test.Controllers.Products.ProductHelper.CreateProduct(this, commandCreateProduct);
            Assert.NotNull(product.CompareGroupId);

            //Delete compareGroup
            var commandDelete = new Delete.Command()
            {
                CompareGroupId = compareGroup.CompareGroupId
            };

            var dbContext = this.GetDbContext();
            var compareGroupDeleteHandler = new Delete.Handler(dbContext);
            await compareGroupDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test product compareGroup is null
            var dbProduct = await ExecuteDbContextAsync(db => db.Products.Where(p => p.ProductId == product.ProductId).SingleOrDefaultAsync());

            Assert.Null(dbProduct.CompareGroupId);
        }
    }
}
