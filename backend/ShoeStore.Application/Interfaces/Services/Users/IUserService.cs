using ShoeStore.Application.DTOs.Accounts;

namespace ShoeStore.Application.Interfaces.Services.Users;

public interface IUserService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task ResetPassword(Guid id, ResetPasswordDto resetPassword, CancellationToken cancellationToken = default);
    Task<AddressDto> UpdateAddressAsync(Guid id, AddressDto addressDto, CancellationToken cancellationToken = default);
}