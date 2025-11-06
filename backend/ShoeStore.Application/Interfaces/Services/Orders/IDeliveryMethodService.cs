using ShoeStore.Application.DTOs.Orders.DeliveryMethods;

namespace ShoeStore.Application.Interfaces.Services.Orders;

public interface IDeliveryMethodService
{
    Task<DeliveryMethodDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeliveryMethodDto>> GetAllAsync(CancellationToken cancellationToken = default);
}