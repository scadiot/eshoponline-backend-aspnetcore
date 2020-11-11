using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Specifications
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int SpecificationId { get; set; }
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
                var specification = await _context.Specifications
                    .FirstOrDefaultAsync(x => x.SpecificationId == message.SpecificationId, cancellationToken);

                if (specification == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { brand = Constants.NOT_FOUND });
                }

                _context.Specifications.Remove(specification);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
