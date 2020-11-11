using eshoponline.Controllers.CompareGroups;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.CompareGroups
{
    public class ListTest : SliceFixture
    {
        [Fact]
        public async Task Expect_List_CompareGroups()
        {
            await MainHelper.CreateRandomData(this);

            //Create brands
            var commandCreate1 = new Create.Command()
            {
                Name = "Computer"
            };
            var brand1 = await CompareGroupHelper.CreateCompareGroup(this, commandCreate1);

            var commandCreate2 = new Create.Command()
            {
                Name = "Smartphone"
            };
            var brand2 = await CompareGroupHelper.CreateCompareGroup(this, commandCreate2);

            var commandCreate3 = new Create.Command()
            {
                Name = "Bike"
            };
            var brand3 = await CompareGroupHelper.CreateCompareGroup(this, commandCreate3);

            //List compareGroups
            var command = new List.Query();
            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();
            var compareGroupsListHandler = new List.Handler(dbContext, mapper);
            var compareGroupsList = await compareGroupsListHandler.Handle(command, new System.Threading.CancellationToken());

            //List compareGroups
            var dbCompareGroups = await ExecuteDbContextAsync(db => db.CompareGroups.ToListAsync());

            Assert.Equal(dbCompareGroups.Count, compareGroupsList.CompareGroups.Count);
        }
    }
}
