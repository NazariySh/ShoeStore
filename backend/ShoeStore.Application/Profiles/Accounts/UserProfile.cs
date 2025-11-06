using AutoMapper;
using ShoeStore.Application.DTOs.Accounts;
using ShoeStore.Domain.Entities.Users;

namespace ShoeStore.Application.Profiles.Accounts;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(src => src.Roles.Select(ur => ur.RoleName)));
    }
}