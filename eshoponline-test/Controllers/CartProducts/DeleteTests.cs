using eshoponline_test.Controllers.Products;
using eshoponline_test.Controllers.Users;
using eshoponline.Controllers.CartProducts;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.CartProducts
{
    public class DeleteTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Delete_CartProduct()
        {
            await MainHelper.CreateRandomData(this);

            //Create user accessor
            var currentUserAccessor = await UserHelper.CreateCurrentUserAccessor(this, true, true);

            // Create product
            var product = await ProductHelper.CreateRandomProduct(this);

            var cartProduct = await CartProductsHelper.CreateCartProducts(this, currentUserAccessor, product.ProductId, 1);
            var command = new Delete.Command()
            {
                CartProductId = cartProduct.CartProductId
            };
            var cartProductCreateHandler = new Delete.Handler(this.GetDbContext(), currentUserAccessor);
            var result = await cartProductCreateHandler.Handle(command, new System.Threading.CancellationToken());

            //Test CartProduct existence
            var dbCartProduct = await CartProductsHelper.GetCartProduct(this, cartProduct.CartProductId);

            Assert.Null(dbCartProduct);
        }
    }
}
