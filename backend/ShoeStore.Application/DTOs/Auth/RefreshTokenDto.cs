namespace ShoeStore.Application.DTOs.Auth;

public record RefreshTokenDto(
    string Token,
    DateTime ExpiryTime);