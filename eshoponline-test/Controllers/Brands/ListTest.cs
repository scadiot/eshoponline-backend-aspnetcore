
using eshoponline.Controllers.Brands;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Brands
{
    public class ListTest : SliceFixture
    {
        [Fact]
        public async Task Expect_List_Brand()
        {
            await MainHelper.CreateRandomData(this);

            //Create brands
            var commandCreate1 = new Create.Command()
            {
                Name = "Nike",
                Slug = "nike"
            };
            var brand1 = await BrandHelper.CreateBrand(this, commandCreate1);

            var commandCreate2 = new Create.Command()
            {
                Name = "Coca Cola",
                Slug = "coca_cola"
            };
            var brand2 = await BrandHelper.CreateBrand(this, commandCreate2);

            var commandCreate3 = new Create.Command()
            {
                Name = "Pepsi",
                Slug = "pepsi"
            };
            var brand3 = await BrandHelper.CreateBrand(this, commandCreate3);

            //List brands
            var command = new List.Query();
            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();
            var brandsListHandler = new List.Handler(dbContext, mapper);
            var brandsList = await brandsListHandler.Handle(command, new System.Threading.CancellationToken());

            //List brands
            var dbBrands = await ExecuteDbContextAsync(db => db.Brands.ToListAsync());

            Assert.Equal(dbBrands.Count, brandsList.Brands.Count);
        }
    }
}
