using ShoeStore.Application.DTOs;
using ShoeStore.Application.DTOs.Shoes;
using ShoeStore.Domain.Models;

namespace ShoeStore.Application.Interfaces.Services.Shoes;

public interface IShoeService
{
    Task<Guid> CreateAsync(ShoeCreateDto shoeCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(ShoeUpdateDto shoeUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ShoeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<ShoeDto>> GetAllAsync(ShoeQuery query, CancellationToken cancellationToken = default);
}