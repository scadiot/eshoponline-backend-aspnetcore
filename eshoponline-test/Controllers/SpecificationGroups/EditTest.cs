using System.Threading.Tasks;
using Xunit;
using eshoponline.Controllers.SpecificationGroups;

namespace eshoponline_test.Controllers.SpecificationGroups
{
    public class EditTest : SliceFixture
    {
        [Fact]
        public async Task Expect_Edit_SpecificationGroups()
        {
            await MainHelper.CreateRandomData(this);

            //Create specification group
            var commandCreate = new Create.Command()
            {
                Name = "performances",
                LongName = "Performances"
            };

            var specificationGroup = await SpecificationGroupHelper.CreateSpecificationGroup(this, commandCreate);

            //Edit specification group
            var commandEdit = new Edit.Command()
            {
                SpecificationGroupId = specificationGroup.SpecificationGroupId,
                Name = "performances 2",
                LongName = "Performances 2"
            };

            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();

            var specificationGroupEditHandler = new Edit.Handler(dbContext, mapper);
            var editedSpecificationGroup = await specificationGroupEditHandler.Handle(commandEdit, new System.Threading.CancellationToken());

            //Test results
            Assert.NotNull(specificationGroup);
            Assert.Equal(commandEdit.Name, editedSpecificationGroup.Name);
            Assert.Equal(commandEdit.LongName, editedSpecificationGroup.LongName);
        }
    }
}
