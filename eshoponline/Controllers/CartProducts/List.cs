using AutoMapper;
using eshoponline.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CartProducts
{
    public class List
    {
        public class CartProductsListDto
        {
            public List<CartProductDto> CartProducts { get; set; }
        }

        public class Query : IRequest<CartProductsListDto>
        {

        }

        public class Handler : IRequestHandler<Query, CartProductsListDto>
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

            public async Task<CartProductsListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                var productReviews = await _context.CartProducts
                    .Where(pr => pr.UserId == _currentUserAccessor.GetCurrentUser().UserId)
                    .Include(pr => pr.Product)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                CartProductsListDto result = new CartProductsListDto()
                {
                    CartProducts = _mapper.Map<List<CartProductDto>>(productReviews)
                };

                return result;
            }
        }
    }
}
