using AutoMapper;
using ShoeStore.Application.DTOs.Shoes.Categories;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Models;

namespace ShoeStore.Application.Profiles.Shoes;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<PagedList<Category>, PagedList<CategoryDto>>();

        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
    }
}