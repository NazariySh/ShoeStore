using AutoMapper;
using ShoeStore.Application.DTOs.Auth;
using ShoeStore.Domain.Entities.Users;

namespace ShoeStore.Application.Profiles.Auth;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterDto, User>();
    }
}