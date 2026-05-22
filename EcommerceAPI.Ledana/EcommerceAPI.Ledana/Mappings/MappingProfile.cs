using AutoMapper;
using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Models;

namespace EcommerceAPI.Ledana.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap< ProductDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<CategoryDto, Category>();
            CreateMap<SaleProductDto, SaleProduct>();
            CreateMap<SaleDto, Sale>();
        }
    }
}
