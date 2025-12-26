using AutoMapper;
using JafnaEcommerceApi.Models.DTOs.SaleDTOs;
using JafnaEcommerceApi.Models.Entities;
namespace JafnaEcommerceApi.AutoMapper;

public class SaleDetailProfile : Profile
{
    public SaleDetailProfile()
    {
        CreateMap<SaleDetail, SaleDetailDto>();
         
       
        CreateMap<SaleDetailDto, SaleDetail>();
    }
}
