using eshoponline.Controllers.Brands;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.Brands
{
    class BrandHelper
    {
        public static async Task<eshoponline.Models.Brand> CreateBrand(SliceFixture fixture, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var brandCreateHandler = new Create.Handler(dbContext, mapper);
            var created = await brandCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbCategory = await fixture.ExecuteDbContextAsync(db => db.Brands.Where(a => a.BrandId == created.BrandId)
                .SingleOrDefaultAsync());

            return dbCategory;
        }

        public static async Task<eshoponline.Models.Brand> GetBrand(SliceFixture fixture, int brandId)
        {
            var brand = await fixture.ExecuteDbContextAsync(db => db.Brands.Where(a => a.BrandId == brandId)
                .SingleOrDefaultAsync());

            return brand;
        }
    }
}
