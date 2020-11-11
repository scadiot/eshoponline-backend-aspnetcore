using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.SpecificationGroups
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int SpecificationGroupId { get; set; }
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
                var specificationGroup = await _context.SpecificationGroups
                    .FirstOrDefaultAsync(x => x.SpecificationGroupId == message.SpecificationGroupId, cancellationToken);

                if (specificationGroup == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { brand = Constants.NOT_FOUND });
                }

                _context.SpecificationGroups.Remove(specificationGroup);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
