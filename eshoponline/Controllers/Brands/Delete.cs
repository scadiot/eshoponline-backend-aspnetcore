using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Brands
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int BrandId { get; set; }
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
                var brand = await _context.Brands
                    .FirstOrDefaultAsync(x => x.BrandId == message.BrandId, cancellationToken);

                if (brand == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { brand = Constants.NOT_FOUND });
                }

                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
