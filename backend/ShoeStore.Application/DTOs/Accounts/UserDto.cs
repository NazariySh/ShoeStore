namespace ShoeStore.Application.DTOs.Accounts;

public class UserDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public AddressDto? Address { get; set; }
    public IReadOnlyList<string> Roles { get; set; } = [];
}