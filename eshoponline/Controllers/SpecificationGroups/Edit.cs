using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.SpecificationGroups
{
    public class Edit
    {
        public class Command : IRequest<SpecificationGroupDto>
        {
            [JsonIgnore]
            public int SpecificationGroupId { get; set; }
            public string Name { get; set; }
            public string LongName { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().NotNull();
                RuleFor(x => x.LongName).NotEmpty().NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, SpecificationGroupDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SpecificationGroupDto> Handle(Command message, CancellationToken cancellationToken)
            {
                var specificationGroup = await _context.SpecificationGroups
                    .Where(x => x.SpecificationGroupId == message.SpecificationGroupId)
                    .FirstOrDefaultAsync(cancellationToken);

                specificationGroup.Name = message.Name;
                specificationGroup.LongName = message.LongName;

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<SpecificationGroupDto>(specificationGroup);
            }
        }
    }
}
