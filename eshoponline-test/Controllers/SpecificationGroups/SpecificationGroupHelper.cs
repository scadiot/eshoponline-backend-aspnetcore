using eshoponline.Controllers.SpecificationGroups;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.SpecificationGroups
{
    class SpecificationGroupHelper
    {
        public static async Task<eshoponline.Models.SpecificationGroup> CreateSpecificationGroup(SliceFixture fixture, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var specificationGroupCreateHandler = new Create.Handler(dbContext, mapper);
            var created = await specificationGroupCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbCategory = await fixture.ExecuteDbContextAsync(db => db.SpecificationGroups.Where(a => a.SpecificationGroupId == created.SpecificationGroupId)
                .SingleOrDefaultAsync());

            return dbCategory;
        }
    }
}
