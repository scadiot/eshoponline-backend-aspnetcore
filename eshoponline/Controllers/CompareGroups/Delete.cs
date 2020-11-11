using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CompareGroups
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int CompareGroupId { get; set; }
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
                var compareGroup = await _context.CompareGroups
                    .FirstOrDefaultAsync(x => x.CompareGroupId == message.CompareGroupId, cancellationToken);

                if (compareGroup == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { compareGroup = Constants.NOT_FOUND });
                }

                _context.CompareGroups.Remove(compareGroup);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
