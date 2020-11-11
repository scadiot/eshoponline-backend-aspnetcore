using eshoponline.Controllers.Orders;
using eshoponline.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.Orders
{
    class OrderHelper
    {
        public static async Task<eshoponline.Models.Order> CreateOrder(SliceFixture fixture, ICurrentUserAccessor currentUserAccessor, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var orderCreateHandler = new Create.Handler(dbContext, mapper, currentUserAccessor);
            var order = await orderCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbOrder = await fixture.ExecuteDbContextAsync(
                db => db.Orders
                .Where(o => o.OrderId == order.OrderId)
                .SingleOrDefaultAsync());

            return dbOrder;
        }

        public static async Task<eshoponline.Models.Order> CreateRandomOrder(SliceFixture fixture, ICurrentUserAccessor currentUserAccessor, int[] productIds)
        {
            foreach(var productId in productIds)
            {
                var cartProductsCreateCommand = new eshoponline.Controllers.CartProducts.Create.Command()
                {
                    ProductId = productId,
                    Quantity = 1
                };
                await CartProducts.CartProductsHelper.CreateCartProducts(fixture, currentUserAccessor, cartProductsCreateCommand);
            }

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
            return await CreateOrder(fixture, currentUserAccessor, command);
        }
    }
}
