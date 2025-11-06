using AutoMapper;
using ShoeStore.Application.DTOs.Shoes.Brands;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Models;

namespace ShoeStore.Application.Profiles.Shoes;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandDto>();
        CreateMap<PagedList<Brand>, PagedList<BrandDto>>();

        CreateMap<BrandCreateDto, Brand>();
        CreateMap<BrandUpdateDto, Brand>();
    }
}