using AutoMapper;
using eshoponline.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.WishlistProducts
{
    public class List
    {
        public class WishlistProductsListDto
        {
            public List<WishlistProductDto> WishlistProducts { get; set; }
        }


        public class Query : IRequest<WishlistProductsListDto>
        {

        }

        public class Handler : IRequestHandler<Query, WishlistProductsListDto>
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

            public async Task<WishlistProductsListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                var wishlistProducts = await _context.WishlistProducts
                    .Where(pr => pr.UserId == _currentUserAccessor.GetCurrentUser().UserId)
                    .Include(pr => pr.Product)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                WishlistProductsListDto result = new WishlistProductsListDto()
                {
                    WishlistProducts = _mapper.Map<List<WishlistProductDto>>(wishlistProducts)
                };

                return result;
            }
        }
    }
}
