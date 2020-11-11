using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Brands
{
    public class List
    {
        public class BrandsListDto
        {
            public List<BrandDto> Brands { get; set; }
        }

        public class Query : IRequest<BrandsListDto>
        {

        }

        public class Handler : IRequestHandler<Query, BrandsListDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BrandsListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<Brand> queryable = _context.Brands;

                var brands = await queryable
                    .OrderByDescending(x => x.Name)
                    .ToListAsync(cancellationToken);

                BrandsListDto result = new BrandsListDto()
                {
                    Brands = _mapper.Map<List<BrandDto>>(brands)
                };

                return result;
            }
        }
    }
}
