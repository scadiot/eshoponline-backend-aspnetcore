using System.Threading.Tasks;
using Xunit;
using eshoponline.Controllers.SpecificationGroups;
using Microsoft.EntityFrameworkCore;

namespace eshoponline_test.Controllers.SpecificationGroups
{
    public class ListTest : SliceFixture
    {
        [Fact]
        public async Task Expect_List_SpecificationGroups()
        {
            await MainHelper.CreateRandomData(this);

            //Create specification groups
            var command1 = new Create.Command()
            {
                Name = "performances 1",
                LongName = "Performances 1"
            };
            var specificationGroup1 = await SpecificationGroupHelper.CreateSpecificationGroup(this, command1);

            var command2 = new Create.Command()
            {
                Name = "performances 2",
                LongName = "Performances 2"
            };
            var specificationGroup2 = await SpecificationGroupHelper.CreateSpecificationGroup(this, command2);

            //List specification group
            var command = new List.Query();
            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();
            var specificationGroupsListHandler = new List.Handler(dbContext, mapper);
            var specificationGroupsList = await specificationGroupsListHandler.Handle(command, new System.Threading.CancellationToken());

            //List specification group
            var dbSpecificationGroups = await ExecuteDbContextAsync(db => db.SpecificationGroups.ToListAsync());

            Assert.Equal(dbSpecificationGroups.Count, specificationGroupsList.SpecificationGroups.Count);
        }
    }
}
