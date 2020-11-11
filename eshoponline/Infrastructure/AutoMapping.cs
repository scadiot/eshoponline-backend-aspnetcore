using AutoMapper;
using System.Linq;

namespace eshoponline.Infrastructure
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<eshoponline.Controllers.Products.Create.Command, eshoponline.Models.Product>()
                .ForMember(p => p.Features, config => config.Ignore())
                .ForMember(p => p.Keywords, config => config.Ignore());
            CreateMap<eshoponline.Models.Product, eshoponline.Controllers.Products.ProductDto>()
                .ForMember(p => p.CategoryIds, config => config.MapFrom(p => p.Categories.Select(pc => pc.CategoryId).ToArray()));

            CreateMap<eshoponline.Controllers.Categories.Create.Command, eshoponline.Models.Category>();
            CreateMap<eshoponline.Models.Category, eshoponline.Controllers.Categories.CategoryDto>();

            CreateMap<eshoponline.Controllers.CompareGroups.Create.Command, eshoponline.Models.CompareGroup>();
            CreateMap<eshoponline.Models.CompareGroup, eshoponline.Controllers.CompareGroups.CompareGroupDto>();

            CreateMap<eshoponline.Controllers.ProductReviews.Create.Command, eshoponline.Models.ProductReview>();
            CreateMap<eshoponline.Models.ProductReview, eshoponline.Controllers.ProductReviews.ProductReviewDto>();

            CreateMap<eshoponline.Controllers.Brands.Create.Command, eshoponline.Models.Brand>();
            CreateMap<eshoponline.Models.Brand, eshoponline.Controllers.Brands.BrandDto>();

            CreateMap<eshoponline.Models.User, eshoponline.Controllers.Users.UserDto>();

            CreateMap<eshoponline.Controllers.SpecificationGroups.Create.Command, eshoponline.Models.SpecificationGroup>();
            CreateMap<eshoponline.Models.SpecificationGroup, eshoponline.Controllers.SpecificationGroups.SpecificationGroupDto>();

            CreateMap<eshoponline.Controllers.Specifications.Create.Command, eshoponline.Models.Specification>();
            CreateMap<eshoponline.Models.Specification, eshoponline.Controllers.Specifications.SpecificationDto>();

            CreateMap<eshoponline.Controllers.CartProducts.Create.Command, eshoponline.Models.CartProduct>();
            CreateMap<eshoponline.Models.CartProduct, eshoponline.Controllers.CartProducts.CartProductDto>();

            CreateMap<eshoponline.Controllers.Orders.Create.Command, eshoponline.Models.Order>();
            CreateMap<eshoponline.Models.Order, eshoponline.Controllers.Orders.OrderDto>();

            CreateMap<eshoponline.Controllers.Orders.Create.CommandAddress, eshoponline.Models.Address>();
            CreateMap<eshoponline.Models.Address, eshoponline.Controllers.Orders.OrderAddressDto>();

            CreateMap<eshoponline.Models.OrderProduct, eshoponline.Controllers.Orders.OrderProductDto>();
        }
    }
}
