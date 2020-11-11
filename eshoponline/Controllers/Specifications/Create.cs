using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Specifications
{
    public class Create
    {
        public class Command : IRequest<SpecificationDto>
        {
            public string Name { get; set; }
            public SpecificationType Type { get; set; }
            public string Unity { get; set; }
            public string LongName { get; set; }
            public int? SpecificationGroupId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Unity).NotNull().NotEmpty();
                RuleFor(x => x.LongName).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, SpecificationDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SpecificationDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var specification = _mapper.Map<Models.Specification>(request);

                await _context.Specifications.AddAsync(specification);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<SpecificationDto>(specification);
            }
        }
    }
}
