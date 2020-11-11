using eshoponline.Controllers.Specifications;
using eshoponline_test.Controllers.SpecificationGroups;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Specifications
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Specifications()
        {
            await MainHelper.CreateRandomData(this);

            var commandCreateGroup = new eshoponline.Controllers.SpecificationGroups.Create.Command()
            {
                Name = "performances",
                LongName = "Performances"
            };

            var specificationGroup = await SpecificationGroupHelper.CreateSpecificationGroup(this, commandCreateGroup);

            var command = new Create.Command()
            {
                Name = "Frequency",
                LongName = "Processor frequency",
                Type = eshoponline.Models.SpecificationType.Decimal,
                Unity = "Ghz",
                SpecificationGroupId = specificationGroup.SpecificationGroupId
            };

            var specificationCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper());
            var specification = await specificationCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(specification);
            Assert.Equal(command.Name, specification.Name);
            Assert.Equal(command.LongName, specification.LongName);
            Assert.Equal(command.Type, specification.Type);
            Assert.Equal(command.Unity, specification.Unity);
            Assert.Equal(command.SpecificationGroupId, specification.SpecificationGroupId);
        }
    }
}
