using ShoeStore.Application.DTOs;
using ShoeStore.Application.DTOs.Shoes.Brands;
using ShoeStore.Domain.Models;

namespace ShoeStore.Application.Interfaces.Services.Shoes;

public interface IBrandService
{
    Task<BrandDto> CreateAsync(BrandCreateDto brandCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(BrandUpdateDto brandUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BrandDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<BrandDto>> GetAllAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default);
}