using eshoponline.Controllers.ProductReviews;
using eshoponline.Infrastructure;
using eshoponline_test.Controllers.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.ProductReviews
{
    class ProductReviewHelper
    {
        public static async Task<eshoponline.Models.ProductReview> CreateProductReview(SliceFixture fixture, ICurrentUserAccessor currentUserAccessor, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();
            var mapper = fixture.GetMapper();

            var productReviewCreateHandler = new Create.Handler(dbContext, mapper, currentUserAccessor);
            var created = await productReviewCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbProductReview = await fixture.ExecuteDbContextAsync(db => db.ProductReviews
                .Where(a => a.ProductReviewId == created.ProductReviewId)
                .SingleOrDefaultAsync());

            return dbProductReview;
        }

        public static async Task<eshoponline.Models.ProductReview> CreateRandomProductReview(SliceFixture fixture, ICurrentUserAccessor currentUserAccessor, int productId)
        {
            var command = new Create.Command()
            {
                ProductId = productId,
                Comment = "random comment",
                Stars = 3,
                Title = "random title"
            };
            return await CreateProductReview(fixture, currentUserAccessor, command);
        }
    }
}
