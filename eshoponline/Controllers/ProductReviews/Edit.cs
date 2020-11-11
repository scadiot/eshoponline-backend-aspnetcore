
using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.ProductReviews
{
    public class Edit
    {
        public class Command : IRequest<ProductReviewDto>
        {
            [JsonIgnore]
            public int ProductReviewId { get; set; }
            public string Title { get; set; }
            public string Comment { get; set; }
            public double Stars { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotNull().NotEmpty();
                RuleFor(x => x.Comment).NotNull().NotEmpty();
                RuleFor(x => x.Stars).InclusiveBetween(0d, 5d);
            }
        }

        public class Handler : IRequestHandler<Command, ProductReviewDto>
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

            public async Task<ProductReviewDto> Handle(Command message, CancellationToken cancellationToken)
            {
                var productReview = await _context.ProductReviews
                    .Where(x => x.ProductReviewId == message.ProductReviewId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (_currentUserAccessor.GetCurrentUser().UserId != productReview.UserId &&
                    !_currentUserAccessor.GetCurrentUser().Roles.Any(r => r.Label == Models.RoleLabel.manager))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = Constants.OPERATION_NOT_PERMITTED });
                }

                productReview.Title = message.Title;
                productReview.Comment = message.Comment;
                productReview.Stars = message.Stars;

                await _context.SaveChangesAsync(cancellationToken);
                await Helper.updateProductStars(_context, productReview.ProductId, cancellationToken);

                return _mapper.Map<ProductReviewDto>(productReview);
            }
        }
    }
}
