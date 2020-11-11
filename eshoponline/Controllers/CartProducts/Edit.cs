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

namespace eshoponline.Controllers.CartProducts
{
    public class Edit
    {
        public class Command : IRequest<CartProductDto>
        {
            [JsonIgnore]
            public int CartProductId { get; set; }
            public int Quantity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1);
            }
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

            public async Task<CartProductDto> Handle(Command message, CancellationToken cancellationToken)
            {
                var cartProduct = await _context.CartProducts
                    .Where(x => x.CartProductId == message.CartProductId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (_currentUserAccessor.GetCurrentUser().UserId != cartProduct.UserId &&
                    !_currentUserAccessor.GetCurrentUser().Roles.Any(r => r.Label == Models.RoleLabel.manager))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = Constants.OPERATION_NOT_PERMITTED });
                }

                cartProduct.Quantity = message.Quantity;

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CartProductDto>(cartProduct);
            }
        }
    }
}
