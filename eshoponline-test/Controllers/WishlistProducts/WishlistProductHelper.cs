using eshoponline.Controllers.WishlistProducts;
using eshoponline.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.WishlistProducts
{
    class WishlistProductHelper
    {
        public static async Task<eshoponline.Models.WishlistProduct> CreateWishlistProduct(SliceFixture fixture, ICurrentUserAccessor currentUserAccessor, int productId)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var specificationCreateHandler = new Add.Handler(dbContext, mapper, currentUserAccessor);
            var command = new Add.Command()
            {
                ProductId = productId
            };
            var added = await specificationCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbWishlistProduct = await fixture.ExecuteDbContextAsync(db => db.WishlistProducts
                .Where(s => s.WishlistProductId == added.WishlistProductId)
                .Include(s => s.Product)
                .SingleOrDefaultAsync());

            return dbWishlistProduct;
        }

        public static async Task<eshoponline.Models.WishlistProduct> GetWishlistProduct(SliceFixture fixture, int wishlistProductId)
        {
            var dbWishlistProduct = await fixture.ExecuteDbContextAsync(db => db.WishlistProducts
                .Where(s => s.WishlistProductId == wishlistProductId)
                .Include(s => s.Product)
                .SingleOrDefaultAsync());

            return dbWishlistProduct;
        }
    }
}
