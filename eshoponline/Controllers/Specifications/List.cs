using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Specifications
{
    public class List
    {
        public class SpecificationsListDto
        {
            public List<SpecificationDto> Specifications { get; set; }
        }

        public class Query : IRequest<SpecificationsListDto>
        {

        }

        public class Handler : IRequestHandler<Query, SpecificationsListDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SpecificationsListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<Specification> queryable = _context.Specifications;

                var specifications = await queryable
                    .ToListAsync(cancellationToken);

                SpecificationsListDto result = new SpecificationsListDto()
                {
                    Specifications = _mapper.Map<List<SpecificationDto>>(specifications)
                };

                return result;
            }
        }
    }
}
