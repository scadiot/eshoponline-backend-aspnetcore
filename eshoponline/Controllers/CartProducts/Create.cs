using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CartProducts
{
    public class Create
    {
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1);
            }
        }

        public class Command : IRequest<CartProductDto>
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public class Handler : IRequestHandler<Command, CartProductDto>
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

            public async Task<CartProductDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var cartProduct = _mapper.Map<Models.CartProduct>(request);
                cartProduct.UserId = _currentUserAccessor.GetCurrentUser().UserId;

                await _context.CartProducts.AddAsync(cartProduct);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CartProductDto>(cartProduct);
            }
        }
    }
}
