using eshoponline.Controllers.Brands;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Brands
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Brand()
        {
            await MainHelper.CreateRandomData(this);

            var command = new Create.Command()
            {
                Name = "Nike",
                Slug = "nike"
            };

            var brandCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var brand = await brandCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(brand);
            Assert.Equal(command.Name, brand.Name);
            Assert.Equal(command.Slug, brand.Slug);
        }
    }
}
