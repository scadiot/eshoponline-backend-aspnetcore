using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Categories
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int CategoryId { get; set; }
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
                var category = await _context.Categories
                    .FirstOrDefaultAsync(x => x.CategoryId == message.CategoryId, cancellationToken);

                if (category == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { category = Constants.NOT_FOUND });
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
