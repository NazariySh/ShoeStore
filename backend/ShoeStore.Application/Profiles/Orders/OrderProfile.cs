using AutoMapper;
using ShoeStore.Application.DTOs.Orders;
using ShoeStore.Domain.Entities.Orders;
using ShoeStore.Domain.Enums;
using ShoeStore.Domain.Models;

namespace ShoeStore.Application.Profiles.Orders;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<PagedList<Order>, PagedList<OrderDto>>();

        CreateMap<OrderCreateDto, Order>();

        CreateMap<OrderItemDto, OrderItem>()
            .ForMember(
                dest => dest.Shoe,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.ShoeId,
                opt => opt.MapFrom(src => src.Shoe.ShoeId));
    }
}