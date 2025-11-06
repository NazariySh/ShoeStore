using AutoMapper;
using ShoeStore.Application.DTOs.Shoes;
using ShoeStore.Application.DTOs.Shoes.ShoeImages;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Models;

namespace ShoeStore.Application.Profiles.Shoes;

public class ShoeProfile : Profile
{
    public ShoeProfile()
    {
        CreateMap<Shoe, ShoeDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ShoeImages));

        CreateMap<ShoeImage, ShoeImageDto>();
        CreateMap<PagedList<Shoe>, PagedList<ShoeDto>>();

        CreateMap<ShoeCreateDto, Shoe>();
        CreateMap<ShoeUpdateDto, Shoe>();
    }
}