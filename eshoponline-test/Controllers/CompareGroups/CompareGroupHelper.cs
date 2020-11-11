using eshoponline.Controllers.CompareGroups;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.CompareGroups
{
    class CompareGroupHelper
    {
        public static async Task<eshoponline.Models.CompareGroup> CreateCompareGroup(SliceFixture fixture, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var compareGroupCreateHandler = new Create.Handler(dbContext, mapper);
            var created = await compareGroupCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbCompareGroup = await fixture.ExecuteDbContextAsync(
                db => db.CompareGroups
                .Where(a => a.CompareGroupId == created.CompareGroupId)
                .SingleOrDefaultAsync());

            return dbCompareGroup;
        }
    }
}
