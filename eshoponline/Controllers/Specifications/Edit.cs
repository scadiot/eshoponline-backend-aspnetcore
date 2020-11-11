using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Specifications
{
    public class Edit
    {
        public class Command : IRequest<SpecificationDto>
        {
            [JsonIgnore]
            public int SpecificationId { get; set; }
            public string Name { get; set; }
            public SpecificationType Type { get; set; }
            public string Unity { get; set; }
            public string LongName { get; set; }
            public int SpecificationGroupId { get; set; }
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

            public async Task<SpecificationDto> Handle(Command message, CancellationToken cancellationToken)
            {
                var specification = await _context.Specifications
                    .Where(x => x.SpecificationId == message.SpecificationId)
                    .FirstOrDefaultAsync(cancellationToken);

                specification.Name = message.Name;

                specification.Type = message.Type;
                specification.Unity = message.Unity;
                specification.LongName = message.LongName;
                specification.SpecificationGroupId = message.SpecificationGroupId;

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<SpecificationDto>(specification);
            }
        }
    }
}
