using eshoponline.Controllers.CartProducts;
using eshoponline.Infrastructure;
using eshoponline_test.Controllers.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.CartProducts
{
    class CartProductsHelper
    {
        public static async Task<eshoponline.Models.CartProduct> CreateCartProducts(SliceFixture fixture, ICurrentUserAccessor currentUserAccessor, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var cartProductCreateHandler = new Create.Handler(dbContext, mapper, currentUserAccessor);
            var created = await cartProductCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbCartProduct = await fixture.ExecuteDbContextAsync(db => db.CartProducts
                .Where(a => a.CartProductId == created.CartProductId)
                .SingleOrDefaultAsync());

            return dbCartProduct;
        }
    }
}
