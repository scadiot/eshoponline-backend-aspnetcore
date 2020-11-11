using eshoponline.Controllers.CompareGroups;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.CompareGroups
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_CompareGroup()
        {
            await MainHelper.CreateRandomData(this);

            var command = new Create.Command()
            {
                Name = "Computer"
            };

            var compareGroupCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var brand = await compareGroupCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(brand);
            Assert.Equal(command.Name, brand.Name);
        }
    }
}