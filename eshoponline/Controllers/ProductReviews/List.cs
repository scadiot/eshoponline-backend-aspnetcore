using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.ProductReviews
{
    public class List
    {
        public class ProductReviewsListDto
        {
            public List<ProductReviewDto> ProductReviews { get; set; }
            public int Limit { get; set; }
            public int Offset { get; set; }
            public int CategoriesCount { get; set; }
        }

        public class Query : IRequest<ProductReviewsListDto>
        {
            public int ProductId { get; set; }
            public int? Limit { get; set; }
            public int? Offset { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProductReviewsListDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ProductReviewsListDto> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<ProductReview> queryable = _context.ProductReviews.Where(pr => pr.ProductId == query.ProductId);

                var productReviewsCount = await queryable.CountAsync();
                var productReviews = await queryable
                    .OrderByDescending(pr => pr.Date)
                    .Skip(query.Offset ?? 0)
                    .Take(query.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                ProductReviewsListDto result = new ProductReviewsListDto()
                {
                    ProductReviews = _mapper.Map<List<ProductReviewDto>>(productReviews),
                    Offset = query.Offset ?? 0,
                    Limit = query.Limit ?? 20,
                    CategoriesCount = productReviewsCount
                };

                return result;
            }
        }
    }
}
