
using eshoponline.Controllers.Orders;
using eshoponline_test.Controllers.Products;
using eshoponline_test.Controllers.Users;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Orders
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Order()
        {
            var currentUserAccessor = await UserHelper.CreateCurrentUserAccessor(this, false, false);

            await MainHelper.CreateRandomData(this);

            //Create product
            var product1 = await ProductHelper.CreateRandomProduct(this);
            var product2 = await ProductHelper.CreateRandomProduct(this);

            //Add product to cart
            await CartProducts.CartProductsHelper.CreateCartProducts(this, currentUserAccessor, product1.ProductId, 1);
            await CartProducts.CartProductsHelper.CreateCartProducts(this, currentUserAccessor, product2.ProductId, 1);

            //Create order
            var command = new Create.Command()
            {
                ShippingAddress = new Create.CommandAddress()
                {
                    FirstName = "Robert",
                    LastName = "Dupont",
                    Street = "21 Rue du labrador",
                    Street2 = "",
                    PostalCode = "33000",
                    City = "Bordeaux",
                    Country = "France"
                },
                BillingAddress = null
            };

            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();

            var orderCreateHandler = new Create.Handler(dbContext, mapper, currentUserAccessor);
            var order = await orderCreateHandler.Handle(command, new System.Threading.CancellationToken());

            //Test order
            Assert.NotNull(order);
            Assert.Equal(2, order.Products.Length);
            Assert.Equal(product1.UnitPrice + product2.UnitPrice, order.ProductsSumPrice);
            Assert.Equal(command.ShippingAddress.FirstName, order.ShippingAddress.FirstName);
        }
    }
}
