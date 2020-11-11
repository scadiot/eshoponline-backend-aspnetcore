using eshoponline.Infrastructure;
using eshoponline.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.ProductReviews
{
    public class Helper
    {
        public static async Task<double> updateProductStars(EshoponlineContext context, int productId, CancellationToken cancellationToken)
        {
            var stars = 0d;

            var productReviews = context.ProductReviews.Where(pr => pr.ProductId == productId);

            if (await productReviews.AnyAsync())
            {
                stars = productReviews
                    .Select(pr => pr.Stars)
                    .Average();
            }

            var product = await context.Products.Where(p => p.ProductId == productId).SingleOrDefaultAsync();
            product.Stars = stars;
            await context.SaveChangesAsync(cancellationToken);

            return stars;
        }
    }
}
