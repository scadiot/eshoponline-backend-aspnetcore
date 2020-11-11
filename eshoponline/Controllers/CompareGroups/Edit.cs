using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CompareGroups
{
    public class Edit
    {
        public class Command : IRequest<CompareGroupDto>
        {
            [JsonIgnore]
            public int CompareGroupId { get; set; }
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, CompareGroupDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CompareGroupDto> Handle(Command message, CancellationToken cancellationToken)
            {
                var compareGroup = await _context.CompareGroups
                    .Where(x => x.CompareGroupId == message.CompareGroupId)
                    .FirstOrDefaultAsync(cancellationToken);

                compareGroup.Name = message.Name;

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CompareGroupDto>(compareGroup);
            }
        }
    }
}
