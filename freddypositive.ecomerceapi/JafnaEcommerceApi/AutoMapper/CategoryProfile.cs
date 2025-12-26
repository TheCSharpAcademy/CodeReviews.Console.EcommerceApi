using AutoMapper;
using JafnaEcommerceApi.Models.DTOs.CategoryDTOs;
using JafnaEcommerceApi.Models.Entities;

namespace JafnaEcommerceApi.AutoMapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s)))); ;
    }
}
