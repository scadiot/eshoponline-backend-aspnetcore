using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.ProductReviews
{
    public class Create
    {
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotNull().NotEmpty();
                RuleFor(x => x.Comment).NotNull().NotEmpty();
                RuleFor(x => x.Stars).InclusiveBetween(0d, 5d);
            }
        }

        public class Command : IRequest<ProductReviewDto>
        {
            public string Title { get; set; }
            public string Comment { get; set; }
            public double Stars { get; set; }
            public int ProductId { get; set; }
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

            public async Task<ProductReviewDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var productReview = _mapper.Map<Models.ProductReview>(request);
                productReview.UserId = _currentUserAccessor.GetCurrentUser().UserId;
                productReview.Date = DateTime.Now;

                await _context.ProductReviews.AddAsync(productReview);
                var value = await _context.SaveChangesAsync(cancellationToken);

                await Helper.updateProductStars(_context, productReview.ProductId, cancellationToken);

                return _mapper.Map<ProductReviewDto>(productReview);
            }
        }
    }
}
