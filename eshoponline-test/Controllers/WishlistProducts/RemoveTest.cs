using eshoponline.Controllers.WishlistProducts;
using eshoponline_test.Controllers.Users;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.WishlistProducts
{
    public class RemoveTest : SliceFixture
    {
        [Fact]
        public async Task Expect_Add_WishlistProducts()
        {
            await MainHelper.CreateRandomData(this);

            //Create user accessor
            var currentUserAccessor = await UserHelper.CreateCurrentUserAccessor(this, true, true);

            // Create product
            var product = await Products.ProductHelper.CreateRandomProduct(this);
            var wishlistProduct = await WishlistProducts.WishlistProductHelper.CreateWishlistProduct(this, currentUserAccessor, product.ProductId);

            var command = new Remove.Command()
            {
                WishlistProductId = wishlistProduct.WishlistProductId,
            };
            var wishlistProductCreateHandler = new Remove.Handler(this.GetDbContext(), currentUserAccessor);
            var result = await wishlistProductCreateHandler.Handle(command, new System.Threading.CancellationToken());

            //Test WishlistProduct existence
            var dbWishlistProduct = await WishlistProducts.WishlistProductHelper.GetWishlistProduct(this, wishlistProduct.WishlistProductId);

            Assert.Null(wishlistProduct);
        }
    }
}
