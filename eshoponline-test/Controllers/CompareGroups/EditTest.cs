using eshoponline.Controllers.CompareGroups;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.CompareGroups
{
    public class EditTest : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_CompareGroups()
        {
            await MainHelper.CreateRandomData(this);

            //Create compareGroup
            var commandCreate = new Create.Command()
            {
                Name = "Computr"
            };
            var compareGroup = await CompareGroupHelper.CreateCompareGroup(this, commandCreate);

            //Edit compareGroup
            var commandEdit = new Edit.Command()
            {
                CompareGroupId = compareGroup.CompareGroupId,
                Name = "Computer"
            };

            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();

            var compareGroupEditHandler = new Edit.Handler(dbContext, mapper);
            var editedCompareGroup = await compareGroupEditHandler.Handle(commandEdit, new System.Threading.CancellationToken());

            //Test results
            Assert.NotNull(editedCompareGroup);
            Assert.Equal(commandEdit.Name, editedCompareGroup.Name);
        }
    }
}
