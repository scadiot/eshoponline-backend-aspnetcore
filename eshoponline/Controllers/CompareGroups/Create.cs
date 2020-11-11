using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CompareGroups
{
    public class Create
    {
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
            }

            private object RuleFor(Func<object, object> p)
            {
                throw new NotImplementedException();
            }
        }

        public class Command : IRequest<CompareGroupDto>
        {
            public string Name { get; set; }
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

            public async Task<CompareGroupDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var compareGroup = _mapper.Map<Models.CompareGroup>(request);

                await _context.CompareGroups.AddAsync(compareGroup);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CompareGroupDto>(compareGroup);
            }
        }
    }
}
