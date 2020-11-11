using AutoMapper;
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
    public class Detail
    {
        public class Query : IRequest<OrderDto>
        {
            public int OrderId { get; set; }
        }

        public class Handler : IRequestHandler<Query, OrderDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(EshoponlineContext context, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _mapper = mapper;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<OrderDto> Handle(Query query, CancellationToken cancellationToken)
            {
                var order = await _context.Orders
                    .Where(o => o.OrderId == query.OrderId)
                    .Include(o => o.ShippingAddress)
                    .Include(o => o.BillingAddress)
                    .Include(o => o.Products)
                    .SingleOrDefaultAsync(cancellationToken);

                if (order == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { order = Constants.NOT_FOUND });
                }

                if (_currentUserAccessor.GetCurrentUser().UserId != order.UserId &&
                    !_currentUserAccessor.GetCurrentUser().Roles.Any(r => r.Label == Models.RoleLabel.manager))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = Constants.OPERATION_NOT_PERMITTED });
                }

                return _mapper.Map<OrderDto>(order);
            }
        }
    }
}
