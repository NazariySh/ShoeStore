using ShoeStore.Domain.Entities.Users;
using System.Security.Claims;
using ShoeStore.Application.DTOs.Auth;

namespace ShoeStore.Application.Interfaces;

public interface ITokenProvider
{
    TokenDto GenerateToken(User user);
    Task<ClaimsPrincipal> GetPrincipalAsync(string accessToken);
}