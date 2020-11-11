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

namespace eshoponline.Controllers.ProductReviews
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int ProductReviewId { get; set; }
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
                var productReview = await _context.ProductReviews
                    .FirstOrDefaultAsync(x => x.ProductReviewId == message.ProductReviewId, cancellationToken);

                if (productReview == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { productReview = Constants.NOT_FOUND });
                }

                if (_currentUserAccessor.GetCurrentUser().UserId != productReview.UserId &&
                    !_currentUserAccessor.GetCurrentUser().Roles.Any(r => r.Label == Models.RoleLabel.manager))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = Constants.OPERATION_NOT_PERMITTED });
                }

                _context.ProductReviews.Remove(productReview);
                await _context.SaveChangesAsync(cancellationToken);

                await Helper.updateProductStars(_context, productReview.ProductId, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
