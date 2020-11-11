using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Categories
{
    public class Edit
    {
        public class Command : IRequest<CategoryDto>
        {
            [JsonIgnore]
            public int CategoryId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Slug { get; set; }
            public int? ParentCategoryId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().NotNull();
                RuleFor(x => x.Description).NotEmpty().NotNull();
                RuleFor(x => x.Slug).NotEmpty().NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, CategoryDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CategoryDto> Handle(Command message, CancellationToken cancellationToken)
            {
                var category = await _context.Categories
                    .Where(x => x.CategoryId == message.CategoryId)
                    .FirstOrDefaultAsync(cancellationToken);

                category.Name = message.Name;
                category.Description = message.Description;
                category.Slug = message.Slug;
                category.ParentCategoryId = message.ParentCategoryId;

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CategoryDto>(category);
            }
        }
    }
}