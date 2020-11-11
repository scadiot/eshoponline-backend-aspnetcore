using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Categories
{
    public class Create
    {
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Description).NotNull().NotEmpty();
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<CategoryDto>
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Slug { get; set; }
            public int? ParentCategoryId { get; set; }
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

            public async Task<CategoryDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var category = _mapper.Map<Models.Category>(request);

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CategoryDto>(category);
            }
        }
    }
}
