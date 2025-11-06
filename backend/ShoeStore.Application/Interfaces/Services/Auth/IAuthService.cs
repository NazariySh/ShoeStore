using ShoeStore.Application.DTOs.Auth;
using ShoeStore.Domain.Enums;

namespace ShoeStore.Application.Interfaces.Services.Auth;

public interface IAuthService
{
    Task<Guid> RegisterAsync(RegisterDto registerDto, RoleType roleType, CancellationToken cancellationToken = default);
    Task<TokenDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<TokenDto> RefreshTokenAsync(string accessToken, CancellationToken cancellationToken = default);
    Task LogoutAsync(Guid id, CancellationToken cancellationToken = default);
}