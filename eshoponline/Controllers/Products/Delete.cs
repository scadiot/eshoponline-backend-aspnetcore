using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Products
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly EshoponlineContext _context;

            public Handler(EshoponlineContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(x => x.ProductId == message.ProductId, cancellationToken);

                if (product == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { category = Constants.NOT_FOUND });
                }

                //Remove category
                _context.Products.Remove(product);

                //Decrement ProductsCount on category
                if (product.MainCategoryId != null)
                {
                    var category = _context.Categories.Where(c => c.CategoryId == product.MainCategoryId).SingleOrDefault();
                    category.ProductsCount--;
                }

                //Update brand products count
                if (product.BrandId != null)
                {
                    var brand = _context.Brands.Where(c => c.BrandId == product.BrandId).SingleOrDefault();
                    brand.ProductsCount--;
                }

                //apply changes
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
