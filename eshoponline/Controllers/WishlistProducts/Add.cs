using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.WishlistProducts
{
    public class Add
    {
        public class Command : IRequest<WishlistProductDto>
        {
            public int ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Command, WishlistProductDto>
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

            public async Task<WishlistProductDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var wishlistProduct = new Models.WishlistProduct()
                {
                    ProductId = request.ProductId,
                    UserId = _currentUserAccessor.GetCurrentUser().UserId
                };

                await _context.WishlistProducts.AddAsync(wishlistProduct);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<WishlistProductDto>(wishlistProduct);
            }
        }
    }
}
