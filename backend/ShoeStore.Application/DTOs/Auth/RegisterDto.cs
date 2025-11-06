namespace ShoeStore.Application.DTOs.Auth;

public record RegisterDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password);