using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoeStore.Domain.Entities.Orders;
using ShoeStore.Domain.Enums;
using ShoeStore.Domain.Repositories.Orders;
using ShoeStore.Infrastructure.Data;

namespace ShoeStore.Infrastructure.Repositories.Orders;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(ShoeStoreDbContext dbContext, IConfiguration configuration)
        : base(dbContext)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        ArgumentNullException.ThrowIfNull(connectionString);
        _connectionString = connectionString;
    }

    public async Task UpdateStatusAsync(
        Guid orderId,
        OrderStatus status,
        CancellationToken cancellationToken = default)
    {
        await DbContext.Orders
            .Where(o => o.OrderId == orderId)
            .ExecuteUpdateAsync(
                o => o.SetProperty(x => x.Status, status.ToString()),
                cancellationToken);
    }

    public async Task<Guid> CreateFromCartAsync(
        Guid shoppingCartId,
        Guid customerId,
        Guid deliveryMethodId,
        Guid? employeeId = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var orderId = await connection.ExecuteScalarAsync<Guid>(
            "EXEC CreateOrderFromCart @ShoppingCartId, @CustomerId, @DeliveryMethodId, @EmployeeId",
            new
            {
                ShoppingCartId = shoppingCartId,
                CustomerId = customerId,
                DeliveryMethodId = deliveryMethodId,
                EmployeeId = employeeId
            });

        return orderId;
    }
}