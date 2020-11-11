using eshoponline.Controllers.Brands;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Brands
{
    public class EditTest : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Brand()
        {
            await MainHelper.CreateRandomData(this);

            //Create brand
            var commandCreate = new Create.Command()
            {
                Name = "Nike",
                Slug = "nike"
            };
            var brand = await BrandHelper.CreateBrand(this, commandCreate);

            //Edit brand
            var commandEdit = new Edit.Command()
            {
                BrandId = brand.BrandId,
                Name = "Nike 2",
                Slug = "nike 2"
            };

            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();

            var brandEditHandler = new Edit.Handler(dbContext, mapper);
            var editedBrand = await brandEditHandler.Handle(commandEdit, new System.Threading.CancellationToken());

            //Test results
            Assert.NotNull(editedBrand);
            Assert.Equal(commandEdit.Name, editedBrand.Name);
            Assert.Equal(commandEdit.Slug, editedBrand.Slug);
        }
    }
}
