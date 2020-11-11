using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Orders
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int OrderId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly EshoponlineContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(EshoponlineContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(x => x.OrderId == message.OrderId, cancellationToken);

                if (order == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { order = Constants.NOT_FOUND });
                }

                if (_currentUserAccessor.GetCurrentUser().UserId != order.UserId &&
                    !_currentUserAccessor.GetCurrentUser().Roles.Any(r => r.Label == Models.RoleLabel.manager))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = Constants.OPERATION_NOT_PERMITTED });
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
