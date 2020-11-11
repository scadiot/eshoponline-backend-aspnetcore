using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.SpecificationGroups
{
    public class List
    {
        public class SpecificationGroupsListDto
        {
            public List<SpecificationGroupDto> SpecificationGroups { get; set; }
        }

        public class Query : IRequest<SpecificationGroupsListDto>
        {

        }

        public class Handler : IRequestHandler<Query, SpecificationGroupsListDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SpecificationGroupsListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<SpecificationGroup> queryable = _context.SpecificationGroups;

                var specificationGroups = await queryable
                    .ToListAsync(cancellationToken);

                SpecificationGroupsListDto result = new SpecificationGroupsListDto()
                {
                    SpecificationGroups = _mapper.Map<List<SpecificationGroupDto>>(specificationGroups)
                };

                return result;
            }
        }
    }
}
