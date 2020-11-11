using eshoponline.Controllers.Brands;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Brands
{
    public class DeleteTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Delete_Brand()
        {
            await MainHelper.CreateRandomData(this);

            //Create brand
            var commandCreate = new Create.Command()
            {
                Name = "Nike",
                Slug = "nike"
            };
            var brand = await BrandHelper.CreateBrand(this, commandCreate);

            //Delete brand
            var commandDelete = new Delete.Command()
            {
                BrandId = brand.BrandId
            };

            var dbContext = this.GetDbContext();
            var brandDeleteHandler = new Delete.Handler(dbContext);
            await brandDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test brand existence
            var dbBrand = await ExecuteDbContextAsync(db => db.Brands.Where(d => d.BrandId == brand.BrandId).SingleOrDefaultAsync());

            Assert.Null(dbBrand);
        }
    }
}
