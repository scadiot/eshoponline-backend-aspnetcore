using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Brands
{
    public class Edit
    {
        public class Command : IRequest<BrandDto>
        {
            [JsonIgnore]
            public int BrandId { get; set; }
            public string Name { get; set; }
            public string Slug { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().NotNull();
                RuleFor(x => x.Slug).NotEmpty().NotNull();
            }
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

            public async Task<BrandDto> Handle(Command message, CancellationToken cancellationToken)
            {
                var brand = await _context.Brands
                    .Where(x => x.BrandId == message.BrandId)
                    .FirstOrDefaultAsync(cancellationToken);

                brand.Name = message.Name;
                brand.Slug = message.Slug;

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<BrandDto>(brand);
            }
        }
    }
}
