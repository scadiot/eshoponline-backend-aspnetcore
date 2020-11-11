using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CompareGroups
{
    public class List
    {
        public class CompareGroupsListDto
        {
            public List<CompareGroupDto> CompareGroups { get; set; }
        }

        public class Query : IRequest<CompareGroupsListDto>
        {

        }

        public class Handler : IRequestHandler<Query, CompareGroupsListDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CompareGroupsListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<CompareGroup> queryable = _context.CompareGroups;

                var compareGroups = await queryable
                    .OrderByDescending(x => x.Name)
                    .ToListAsync(cancellationToken);

                CompareGroupsListDto result = new CompareGroupsListDto()
                {
                    CompareGroups = _mapper.Map<List<CompareGroupDto>>(compareGroups)
                };

                return result;
            }
        }
    }
}
