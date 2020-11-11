using AutoMapper;
using eshoponline.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Products
{
    public class Create
    {
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Description).NotNull().NotEmpty();
                RuleFor(x => x.Summary).NotNull().NotEmpty();
            }
        }

        public class ProductSepcification
        {
            public int IntegerValue { get; set; }
            public decimal DecimalValue { get; set; }
            public bool BooleanValue { get; set; }
            public string StringValue { get; set; }
            public int SpecificationId { get; set; }
        }

        public class Command : IRequest<ProductDto>
        {
            public string Name { get; set; }
            public string Slug { get; set; }
            public string Summary { get; set; }
            public string Description { get; set; }
            public decimal UnitPrice { get; set; }
            public int UnitsInStock { get; set; }
            public int? MainCategoryId { get; set; }
            public int[] CategoriesIds { get; set; }
            public int? BrandId { get; set; }
            public int? CompareGroupId { get; set; }

            public string[] Keywords { get; set; }
            public string[] Features { get; set; }

            public ProductSepcification[] ProductSepcifications { get; set; }
        }

        public class Handler : IRequestHandler<Command, ProductDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(EshoponlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = _mapper.Map<Models.Product>(request);
                product.InsertDate = DateTime.Now;

                //Add product
                _context.Products.Add(product);

                //Increment ProductsCount on category
                if (request.MainCategoryId != null)
                {
                    var category = _context.Categories.Where(c => c.CategoryId == request.MainCategoryId).SingleOrDefault();
                    category.ProductsCount++;
                }

                //Update brand products count
                if (request.BrandId != null)
                {
                    var brand = _context.Brands.Where(c => c.BrandId == request.BrandId).SingleOrDefault();
                    brand.ProductsCount++;
                }

                //Apply changes
                await _context.SaveChangesAsync(cancellationToken);

                //Add category list
                if (request.CategoriesIds != null)
                {
                    foreach (var categoryId in request.CategoriesIds)
                    {
                        var productCategory = new Models.ProductCategory()
                        {
                            CategoryId = categoryId,
                            ProductId = product.ProductId
                        };
                        _context.ProductCategories.Add(productCategory);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }

                //Add Features
                if (request.Features != null)
                {
                    foreach (var productFeatures in request.Features)
                    {
                        var productFeature = new Models.ProductFeature()
                        {
                            ProductId = product.ProductId,
                            Text = productFeatures
                        };
                        _context.ProductFeatures.Add(productFeature);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }

                //Add Keywords
                if (request.Keywords != null)
                {
                    foreach (var keyword in request.Keywords)
                    {
                        var dbKyword = _context.Keywords.Where(c => c.Value == keyword).SingleOrDefault();
                        if (dbKyword == null)
                        {
                            dbKyword = new Models.Keyword()
                            {
                                Value = keyword
                            };
                            _context.Keywords.Add(dbKyword);
                            await _context.SaveChangesAsync(cancellationToken);
                        }
                        var productKeyword = new Models.ProductKeyword()
                        {
                            KeywordId = dbKyword.KeywordId,
                            ProductId = product.ProductId
                        };
                        _context.ProductKeywords.Add(productKeyword);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }

                //Add specifications
                if (request.ProductSepcifications != null)
                {
                    foreach (var productSepcification in request.ProductSepcifications)
                    {
                        var dbBroductSpecification = new Models.ProductSpecification()
                        {
                            ProductId = product.ProductId,
                            BooleanValue = productSepcification.BooleanValue,
                            IntegerValue = productSepcification.IntegerValue,
                            StringValue = productSepcification.StringValue,
                            DecimalValue = productSepcification.DecimalValue,
                            SpecificationId = productSepcification.SpecificationId
                        };
                        _context.ProductSpecifications.Add(dbBroductSpecification);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }

                product = await _context.Products
                    .Where(p => p.ProductId == product.ProductId)
                    .Include(p => p.Categories)
                    .Include(p => p.Features)
                    .Include(p => p.Keywords)
                    .Include(p => p.Specifications)
                    .SingleOrDefaultAsync();

                return _mapper.Map<ProductDto>(product);
            }
        }
    }
}
