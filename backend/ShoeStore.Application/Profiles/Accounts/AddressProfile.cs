using AutoMapper;
using ShoeStore.Application.DTOs.Accounts;
using ShoeStore.Domain.Entities.Users;

namespace ShoeStore.Application.Profiles.Accounts;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}