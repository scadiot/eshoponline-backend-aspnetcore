using System.Threading.Tasks;
using Xunit;
using eshoponline.Controllers.SpecificationGroups;

namespace eshoponline_test.Controllers.SpecificationGroups
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_SpecificationGroups()
        {
            await MainHelper.CreateRandomData(this);

            var command = new Create.Command()
            {
                Name = "performances",
                LongName = "Performances"
            };

            var specificationGroupCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var specificationGroup = await specificationGroupCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(specificationGroup);
            Assert.Equal(command.Name, specificationGroup.Name);
            Assert.Equal(command.LongName, specificationGroup.LongName);
        }
    }
}
