using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.SpecificationGroups
{
    public class Create
    {
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.LongName).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<SpecificationGroupDto>
        {
            public string Name { get; set; }
            public string LongName { get; set; }
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

            public async Task<SpecificationGroupDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var specificationGroup = _mapper.Map<Models.SpecificationGroup>(request);

                await _context.SpecificationGroups.AddAsync(specificationGroup);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<SpecificationGroupDto>(specificationGroup);
            }
        }
    }
}
