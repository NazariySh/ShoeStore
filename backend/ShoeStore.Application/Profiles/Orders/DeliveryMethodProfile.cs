using AutoMapper;
using ShoeStore.Application.DTOs.Orders.DeliveryMethods;
using ShoeStore.Domain.Entities.Orders;

namespace ShoeStore.Application.Profiles.Orders;

public class DeliveryMethodProfile : Profile
{
    public DeliveryMethodProfile()
    {
        CreateMap<DeliveryMethod, DeliveryMethodDto>();
    }
}