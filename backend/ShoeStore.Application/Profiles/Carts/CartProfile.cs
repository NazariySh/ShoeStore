using AutoMapper;
using ShoeStore.Application.DTOs.Carts;
using ShoeStore.Domain.Entities.Carts;

namespace ShoeStore.Application.Profiles.Carts;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<ShoppingCart, ShoppingCartDto>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src.CartItems))
            .ReverseMap();

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Product.Brand.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Product.Category.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(
                    src => src.Product.ShoeImages.Where(x => x.IsMain).Select(x => x.Url).FirstOrDefault()));

        CreateMap<CartItemDto, CartItem>();
    }
}