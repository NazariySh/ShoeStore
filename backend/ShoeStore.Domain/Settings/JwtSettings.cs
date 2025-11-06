namespace ShoeStore.Domain.Settings;

public class JwtSettings
{
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int AccessTokenExpiryInMinutes { get; init; }
    public int RefreshTokenExpiryInDays { get; init; }
}