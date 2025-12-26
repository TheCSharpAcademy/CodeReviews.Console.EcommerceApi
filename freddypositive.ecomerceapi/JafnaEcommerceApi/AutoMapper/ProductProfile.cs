using AutoMapper;
using JafnaEcommerceApi.Models.DTOs.ProductDTOs;
using JafnaEcommerceApi.Models.Entities;

namespace JafnaEcommerceApi.AutoMapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();     
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>()
                .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
    }
}
