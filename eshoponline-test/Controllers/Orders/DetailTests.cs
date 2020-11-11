using eshoponline.Controllers.Brands;
using eshoponline.Controllers.Orders;
using eshoponline_test.Controllers.Products;
using eshoponline_test.Controllers.Users;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Orders
{
    public class DetailTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Order()
        {
            var currentUserAccessor = await UserHelper.CreateCurrentUserAccessor(this, false, false);

            await MainHelper.CreateRandomData(this);

            //Create product
            var product1 = await ProductHelper.CreateRandomProduct(this);
            var product2 = await ProductHelper.CreateRandomProduct(this);

            var productIds = new int[] { product1.ProductId, product2.ProductId };

            //Create order
            var order = await OrderHelper.CreateRandomOrder(this, currentUserAccessor, productIds);

            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();

            var query = new Detail.Query()
            {
                OrderId = order.OrderId
            };
            var orderDetailHandler = new Detail.Handler(dbContext, mapper, currentUserAccessor);
            var orderDetail = await orderDetailHandler.Handle(query, new System.Threading.CancellationToken());

            //Test order
            Assert.NotNull(order);
            Assert.Equal(2, orderDetail.Products.Length);
            Assert.Equal(product1.UnitPrice + product2.UnitPrice, order.ProductsSumPrice);
        }
    }
}
