using AutoMapper;
using JafnaEcommerceApi.Models.DTOs.SaleDTOs;
using JafnaEcommerceApi.Models.Entities;

namespace JafnaEcommerceApi.AutoMapper;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<Sale, SaleDto>();

    }

}
