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
        public static async Task<eshoponline.Models.CartProduct> CreateCartProducts(SliceFixture fixture, ICurrentUserAccessor currentUserAccessor, int cartProductId, int quantity)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var cartProductCreateHandler = new Create.Handler(dbContext, mapper, currentUserAccessor);
            Create.Command command = new Create.Command()
            {
                ProductId = cartProductId,
                Quantity = quantity
            };
            var created = await cartProductCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbCartProduct = await fixture.ExecuteDbContextAsync(db => db.CartProducts
                .Where(a => a.CartProductId == created.CartProductId)
                .SingleOrDefaultAsync());

            return dbCartProduct;
        }

        public static async Task<eshoponline.Models.CartProduct> GetCartProduct(SliceFixture fixture, int cartProductId)
        {
            var dbCartProduct = await fixture.ExecuteDbContextAsync(db => db.CartProducts.Where(a => a.CartProductId == cartProductId)
                                          .SingleOrDefaultAsync());

            return dbCartProduct;
        }
    }
}
