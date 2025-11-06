using ShoeStore.Application.DTOs.Orders;
using ShoeStore.Domain.Enums;
using ShoeStore.Domain.Models;

namespace ShoeStore.Application.Interfaces.Services.Orders;

public interface IOrderService
{
    Task<Guid> CreateAsync(OrderCreateDto orderCreateDto, Guid userId, CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken = default);
    Task<OrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<OrderDto>> GetAllAsync(OrderQuery query, CancellationToken cancellationToken = default);
    Task<PagedList<OrderDto>> GetAllAsync(OrderQuery query, Guid userId, CancellationToken cancellationToken = default);
}