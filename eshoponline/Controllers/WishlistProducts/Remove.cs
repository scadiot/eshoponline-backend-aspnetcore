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

namespace eshoponline.Controllers.WishlistProducts
{
    public class Remove
    {
        public class Command : IRequest
        {
            public int WishlistProductId { get; set; }
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
                var wishlistProduct = await _context.WishlistProducts
                    .FirstOrDefaultAsync(x => x.WishlistProductId == message.WishlistProductId, cancellationToken);

                if (wishlistProduct == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { cartProduct = Constants.NOT_FOUND });
                }

                if (_currentUserAccessor.GetCurrentUser().UserId != wishlistProduct.UserId &&
                   !_currentUserAccessor.GetCurrentUser().Roles.Any(r => r.Label == Models.RoleLabel.manager))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = Constants.OPERATION_NOT_PERMITTED });
                }

                _context.WishlistProducts.Remove(wishlistProduct);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
