using eshoponline.Models;
using eshoponline_test.Controllers.Categories;
using eshoponline_test.Controllers.ProductReviews;
using eshoponline_test.Controllers.Products;
using eshoponline_test.Controllers.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers
{
    class MainHelper
    {
        public static async Task CreateRandomData(SliceFixture fixture)
        {
            var currentUserAccessor = await UserHelper.CreateCurrentUserAccessor(fixture, true, true);

            var mainCategory = await CategoryHelper.CreateRandomCategory(fixture);
            var childCategory1 = await CategoryHelper.CreateRandomCategory(fixture, mainCategory.CategoryId);
            var childCategory2 = await CategoryHelper.CreateRandomCategory(fixture, mainCategory.CategoryId);

            var categoriesList1 = new int[] { mainCategory.CategoryId, childCategory1.CategoryId };
            var categoriesList2 = new int[] { mainCategory.CategoryId, childCategory2.CategoryId };

            var product1 = await ProductHelper.CreateRandomProduct(fixture, childCategory1.CategoryId, categoriesList1);
            await ProductReviewHelper.CreateRandomProductReview(fixture, currentUserAccessor, product1.ProductId);
            await ProductReviewHelper.CreateRandomProductReview(fixture, currentUserAccessor, product1.ProductId);
            await ProductReviewHelper.CreateRandomProductReview(fixture, currentUserAccessor, product1.ProductId);

            var product2 = await ProductHelper.CreateRandomProduct(fixture, childCategory2.CategoryId, categoriesList2);
            await ProductReviewHelper.CreateRandomProductReview(fixture, currentUserAccessor, product2.ProductId);
            await ProductReviewHelper.CreateRandomProductReview(fixture, currentUserAccessor, product2.ProductId);
        }
    }
}
