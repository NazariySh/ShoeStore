using Microsoft.EntityFrameworkCore.Query;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Enums;

namespace ShoeStore.Domain.Repositories.Users;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(
        string email,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        CancellationToken cancellationToken = default);

    Task AddToRoleAsync(
        User user,
        RoleType roleType,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task UpdateTokenAsync(
        Guid userId,
        string? refreshToken,
        CancellationToken cancellationToken = default);

    Task UpdateTokenAsync(
        Guid userId,
        string? refreshToken,
        DateTime? refreshTokenExpiryTime,
        CancellationToken cancellationToken = default);

    Task UpdateAddressAsync(
        Address address,
        CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
}