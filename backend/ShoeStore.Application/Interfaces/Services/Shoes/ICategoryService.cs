using ShoeStore.Application.DTOs;
using ShoeStore.Application.DTOs.Shoes.Categories;
using ShoeStore.Domain.Models;

namespace ShoeStore.Application.Interfaces.Services.Shoes;

public interface ICategoryService
{
    Task<CategoryDto> CreateAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<CategoryDto>> GetAllAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
}