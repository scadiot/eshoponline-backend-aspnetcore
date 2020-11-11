using eshoponline.Controllers.ProductReviews;
using eshoponline_test.Controllers.Products;
using eshoponline_test.Controllers.Users;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.ProductReviews
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_ProductReview()
        {
            await MainHelper.CreateRandomData(this);

            //Create user accessor
            var currentUserAccessor = await UserHelper.CreateCurrentUserAccessor(this, true, true);

            // Create product
            var product = await ProductHelper.CreateRandomProduct(this);

            // Create product review
            var command = new Create.Command()
            {
                Title = "Good product",
                Comment = "Comment",
                Stars = 4d,
                ProductId = product.ProductId
            };

            var productReviewCreateHandler = new Create.Handler(this.GetDbContext(), this.GetMapper(), currentUserAccessor);
            var productReview = await productReviewCreateHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(productReview);
            Assert.Equal(command.Title, productReview.Title);
            Assert.Equal(command.Comment, productReview.Comment);
            Assert.Equal(command.Stars, productReview.Stars);
        }
    }
}
