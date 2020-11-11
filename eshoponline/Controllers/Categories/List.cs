using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Categories
{
    public class List
    {
        public class CategoriesListDto
        {
            public List<CategoryDto> Categories { get; set; }
            public int Limit { get; set; }
            public int Offset { get; set; }
            public int CategoriesCount { get; set; }
        }

        public class Query : IRequest<CategoriesListDto>
        {
            public int? Limit { get; set; }
            public int? Offset { get; set; }
        }

        public class Handler : IRequestHandler<Query, CategoriesListDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CategoriesListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                int categoriesCount = await _context.Categories.CountAsync();

                IQueryable<Category> queryable = _context.Categories;
                var categories = await queryable
                    .OrderByDescending(x => x.Name)
                    .Skip(query.Offset ?? 0)
                    .Take(query.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                CategoriesListDto result = new CategoriesListDto()
                {
                    Categories = _mapper.Map<List<CategoryDto>>(categories),
                    Offset = query.Offset ?? 0,
                    Limit = query.Limit ?? 20,
                    CategoriesCount = categoriesCount
                };

                return result;
            }
        }
    }
}
