using eshoponline.Controllers.WishlistProducts;
using eshoponline_test.Controllers.Users;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.WishlistProducts
{
    public class AddTest : SliceFixture
    {
        [Fact]
        public async Task Expect_Add_WishlistProducts()
        {
            await MainHelper.CreateRandomData(this);

            //Create user accessor
            var currentUserAccessor = await UserHelper.CreateCurrentUserAccessor(this, true, true);

            // Create product
            var product = await Products.ProductHelper.CreateRandomProduct(this);

            var command = new Add.Command()
            {
                ProductId = product.ProductId,
            };
            var wishlistProductCreateHandler = new Add.Handler(this.GetDbContext(), this.GetMapper(), currentUserAccessor);
            var wishlistProduct = await wishlistProductCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(wishlistProduct);
        }
    }
}
