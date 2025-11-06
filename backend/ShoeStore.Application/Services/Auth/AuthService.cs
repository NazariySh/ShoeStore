using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Application.DTOs.Auth;
using ShoeStore.Application.Interfaces;
using ShoeStore.Application.Interfaces.Services.Auth;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Enums;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Services.Auth;

public class AuthService : IAuthService
{
    public const string RefreshTokenCookieKey = "refreshToken";

    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public AuthService(
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider,
        IPasswordHasher passwordHasher,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TokenDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(
            loginDto.Email,
            include: x => x
                .Include(u => u.Roles),
            cancellationToken);

        if (user is null || !IsPasswordValid(user, loginDto.Password))
        {
            throw new ArgumentException("Invalid email/password");
        }

        return await CreateTokenAsync(user);
    }

    public async Task<Guid> RegisterAsync(RegisterDto registerDto, RoleType roleType, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(registerDto);

        user.PasswordHash = _passwordHasher.Hash(registerDto.Password);

        user.RefreshToken = new RefreshToken
        {
            Token = null,
            ExpiryTime = null
        };

        var entity = _unitOfWork.Users.Add(user);

        await _unitOfWork.Users.AddToRoleAsync(
            entity,
            roleType,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.UserId;
    }

    public async Task<TokenDto> RefreshTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        var refreshToken = GetRefreshToken();

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        var email = await GetUserEmail(accessToken);

        var user = await _unitOfWork.Users.GetByEmailAsync(
            email,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"User with email {email} not found");

        if (!IsRefreshTokenValid(user, refreshToken))
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        return await CreateTokenAsync(user);
    }

    public async Task LogoutAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.Users.UpdateTokenAsync(
            id,
            null,
            cancellationToken);
    }

    private bool IsPasswordValid(User user, string password)
    {
        return _passwordHasher.Verify(password, user.PasswordHash);
    }

    private async Task<TokenDto> CreateTokenAsync(User user)
    {
        var token = _tokenProvider.GenerateToken(user);

        await _unitOfWork.Users.UpdateTokenAsync(
            user.UserId,
            token.RefreshToken.Token,
            token.RefreshToken.ExpiryTime);

        return token;
    }

    private string? GetRefreshToken()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.Request.Cookies[RefreshTokenCookieKey];
    }

    private async Task<string> GetUserEmail(string accessToken)
    {
        var principal = await _tokenProvider.GetPrincipalAsync(accessToken);
        return principal.Identity?.Name ?? string.Empty;
    }

    private static bool IsRefreshTokenValid(User user, string refreshToken)
    {
        return user.RefreshToken is { Token: not null } &&
               user.RefreshToken.Token.Equals(refreshToken) &&
               user.RefreshToken.ExpiryTime > DateTime.UtcNow;
    }
}