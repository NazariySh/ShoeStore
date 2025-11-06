using Microsoft.Extensions.Options;
using ShoeStore.Application.DTOs.Auth;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Settings;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ShoeStore.Application.Interfaces;

namespace ShoeStore.Infrastructure.Services;

public class JwtTokenProvider : ITokenProvider
{
    public const int RefreshTokenLength = 64;

    private const string Algorithm = SecurityAlgorithms.HmacSha256;

    private readonly JwtSettings _jwtSettings;
    private readonly JsonWebTokenHandler _jsonWebTokenHandler;
    private readonly SymmetricSecurityKey _secureKey;
    private readonly TokenValidationParameters _validationParameters;

    public JwtTokenProvider(IOptions<JwtSettings> jwtSettings)
    {
        ArgumentNullException.ThrowIfNull(jwtSettings);

        _jwtSettings = jwtSettings.Value;

        _jsonWebTokenHandler = new JsonWebTokenHandler();

        ArgumentException.ThrowIfNullOrEmpty(_jwtSettings.Key);

        _secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = _secureKey
        };
    }

    public TokenDto GenerateToken(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        return new TokenDto(accessToken, refreshToken);
    }

    private string GenerateAccessToken(User user)
    {
        var claims = GetClaims(user);
        var tokenDescriptor = GetTokenDescriptor(claims);

        return _jsonWebTokenHandler.CreateToken(tokenDescriptor);
    }

    private RefreshTokenDto GenerateRefreshToken()
    {
        var token = GenerateRandomToken();
        var expiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays);

        return new RefreshTokenDto(token, expiryTime);
    }

    public async Task<ClaimsPrincipal> GetPrincipalAsync(string accessToken)
    {
        var result = await _jsonWebTokenHandler.ValidateTokenAsync(
            accessToken,
            _validationParameters);

        if (!result.IsValid)
        {
            throw new ArgumentException("Invalid token");
        }

        return new ClaimsPrincipal(result.ClaimsIdentity);
    }

    private static string GenerateRandomToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(RefreshTokenLength);
        return Convert.ToBase64String(randomBytes);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(IEnumerable<Claim> claims)
    {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryInMinutes),
            SigningCredentials = new SigningCredentials(_secureKey, Algorithm),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
        };
    }

    private static List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email)
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.RoleName)));

        return claims;
    }
}