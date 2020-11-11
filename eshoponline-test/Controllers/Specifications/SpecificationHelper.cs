using eshoponline.Controllers.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.Specifications
{
    class SpecificationHelper
    {
        public static async Task<eshoponline.Models.Specification> CreateSpecification(SliceFixture fixture, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var specificationCreateHandler = new Create.Handler(dbContext, mapper);
            var created = await specificationCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbSpecification = await fixture.ExecuteDbContextAsync(db => db.Specifications
                .Where(s => s.SpecificationId == created.SpecificationId)
                .Include(s => s.SpecificationGroup)
                .SingleOrDefaultAsync());

            return dbSpecification;
        }

        public static async Task<eshoponline.Models.Specification> GetSpecification(SliceFixture fixture, int specificationId)
        {
            var dbSpecification = await fixture.ExecuteDbContextAsync(db => db.Specifications
                .Where(s => s.SpecificationId == specificationId)
                .Include(s => s.SpecificationGroup)
                .SingleOrDefaultAsync());

            return dbSpecification;
        }
    }
}
