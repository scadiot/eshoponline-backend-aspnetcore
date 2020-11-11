using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Orders
{
    public class List
    {
        public class OrdersListDto
        {
            public List<OrderDto> Orders { get; set; }
        }

        public class Query : IRequest<OrdersListDto>
        {

        }

        public class Handler : IRequestHandler<Query, OrdersListDto>
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

            public async Task<OrdersListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<Order> queryable = _context.Orders.Where(o => o.UserId == _currentUserAccessor.GetCurrentUser().UserId);

                var orders = await queryable
                    .OrderByDescending(x => x.Date)
                    .ToListAsync(cancellationToken);

                OrdersListDto result = new OrdersListDto()
                {
                    Orders = _mapper.Map<List<OrderDto>>(orders)
                };

                return result;
            }
        }
    }
}
