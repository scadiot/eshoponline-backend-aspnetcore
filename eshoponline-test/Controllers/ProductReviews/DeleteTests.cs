using eshoponline.Controllers.ProductReviews;
using eshoponline_test.Controllers.Products;
using eshoponline_test.Controllers.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.ProductReviews
{
    public class DeleteTests : SliceFixture
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
            var commandCreateProductReview = new Create.Command()
            {
                Title = "Good product",
                Comment = "Comment",
                Stars = 4d,
                ProductId = product.ProductId
            };
            var productReview = await ProductReviewHelper.CreateProductReview(this, currentUserAccessor, commandCreateProductReview);
            var dbProductReview1 = await ExecuteDbContextAsync(db => db.ProductReviews
                .Where(d => d.ProductReviewId == productReview.ProductReviewId)
                .SingleOrDefaultAsync());

            //Delete product review
            var commandDelete = new Delete.Command()
            {
                ProductReviewId = productReview.ProductReviewId
            };

            var dbContext = this.GetDbContext();
            var productReviewDeleteHandler = new Delete.Handler(dbContext, currentUserAccessor);
            await productReviewDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test product review existence
            var dbProductReview = await ExecuteDbContextAsync(db => db.ProductReviews
                .Where(d => d.ProductReviewId == productReview.ProductReviewId)
                .SingleOrDefaultAsync());

            Assert.Null(dbProductReview);
        }
    }
}
