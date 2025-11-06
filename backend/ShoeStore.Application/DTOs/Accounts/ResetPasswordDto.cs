namespace ShoeStore.Application.DTOs.Accounts;

public record ResetPasswordDto(
    string CurrentPassword,
    string NewPassword);