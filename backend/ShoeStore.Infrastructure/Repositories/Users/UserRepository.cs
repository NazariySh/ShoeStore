using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Enums;
using ShoeStore.Domain.Repositories.Users;
using ShoeStore.Infrastructure.Data;

namespace ShoeStore.Infrastructure.Repositories.Users;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(ShoeStoreDbContext dbContext, IConfiguration configuration)
        : base(dbContext)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        ArgumentNullException.ThrowIfNull(connectionString);
        _connectionString = connectionString;
    }

    public Task<User?> GetByEmailAsync(
        string email,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        CancellationToken cancellationToken = default)
    {
        return GetSingleAsync(
            x => x.Email.ToLower() == email.ToLower(),
            include: include,
            cancellationToken: cancellationToken);
    }

    public async Task AddToRoleAsync(
        User user,
        RoleType roleType,
        CancellationToken cancellationToken = default)
    {
        var roleName = roleType.ToString();

        var role = await DbContext.Roles.FirstOrDefaultAsync(
            x => x.RoleName == roleName,
            cancellationToken);

        if (role is not null)
        {
            user.Roles.Add(role);
        }
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var roles = await DbContext.Users
            .Where(x => x.UserId == userId)
            .SelectMany(x => x.Roles)
            .Select(x => x.RoleName)
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task UpdateTokenAsync(
        Guid userId,
        string? refreshToken,
        CancellationToken cancellationToken = default)
    {
        await DbContext.RefreshTokens
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.Token, refreshToken),
                cancellationToken);
    }

    public async Task UpdateTokenAsync(
        Guid userId,
        string? refreshToken,
        DateTime? refreshTokenExpiryTime,
        CancellationToken cancellationToken = default)
    {
        await DbContext.RefreshTokens
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.Token, refreshToken)
                    .SetProperty(u => u.ExpiryTime, refreshTokenExpiryTime),
                cancellationToken);
    }

    public async Task UpdateAddressAsync(
        Address address,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await connection.ExecuteAsync(
            "EXEC UpdateAddress @UserId, @Street, @City, @State, @Country, @PostalCode",
            new
            {
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode
            });
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await DbContext.Users.AnyAsync(
            x => x.Email.ToLower() == email.ToLower(),
            cancellationToken);
    }
}