using ShoeStore.Domain.Entities.Orders;
using ShoeStore.Domain.Enums;

namespace ShoeStore.Domain.Repositories.Orders;

public interface IOrderRepository : IRepository<Order>
{
    Task UpdateStatusAsync(
        Guid orderId,
        OrderStatus status,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateFromCartAsync(
        Guid shoppingCartId,
        Guid customerId,
        Guid deliveryMethodId,
        Guid? employeeId = null,
        CancellationToken cancellationToken = default);
}