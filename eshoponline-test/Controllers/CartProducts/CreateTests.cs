using eshoponline.Controllers.CartProducts;
using eshoponline_test.Controllers.Users;
using eshoponline_test.Controllers.Products;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.CartProducts
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_CartProduct()
        {
            await MainHelper.CreateRandomData(this);

            //Create user accessor
            var currentUserAccessor = await UserHelper.CreateCurrentUserAccessor(this, true, true);

            // Create product
            var product = await ProductHelper.CreateRandomProduct(this);

            var command = new Create.Command()
            {
                ProductId = product.ProductId,
                Quantity = 2
            };
            var cartProductCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper(), currentUserAccessor);
            var cartProduct = await cartProductCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(cartProduct);
            Assert.Equal(command.Quantity, cartProduct.Quantity);
        }
    }
}
