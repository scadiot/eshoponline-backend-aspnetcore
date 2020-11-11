using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Brands
{
    public class Create
    {
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<BrandDto>
        {
            public string Name { get; set; }
            public string Slug { get; set; }
        }

        public class Handler : IRequestHandler<Command, BrandDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BrandDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var brand = _mapper.Map<Models.Brand>(request);

                await _context.Brands.AddAsync(brand);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<BrandDto>(brand);
            }
        }
    }
}
