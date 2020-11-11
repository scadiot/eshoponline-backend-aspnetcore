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

namespace eshoponline.Controllers.Products
{
    public class List
    {
        public class ProductsListDto {
            public List<ProductDto> Products { get; set; }
            public int Offset { get; set; }
            public int Limit { get; set; }
            public int ProductsCount { get; set; }
        }

        public class Query : IRequest<ProductsListDto>
        {
            public int? Limit { get; set; }
            public int? Offset { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProductsListDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ProductsListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                int productsCount = await _context.Products.CountAsync();

                IQueryable<Product> queryable = _context.Products;
                var products = await queryable
                    .OrderByDescending(x => x.InsertDate)
                    .Skip(query.Offset ?? 0)
                    .Take(query.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                ProductsListDto result = new ProductsListDto()
                {
                    Products = _mapper.Map<List<ProductDto>>(products),
                    Offset = query.Offset ?? 0,
                    Limit = query.Limit ?? 20,
                    ProductsCount = productsCount
                };

                return result;
            }
        }
    }
}
