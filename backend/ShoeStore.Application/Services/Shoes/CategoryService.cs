using AutoMapper;
using ShoeStore.Application.DTOs;
using ShoeStore.Application.DTOs.Shoes.Categories;
using ShoeStore.Application.Interfaces.Services;
using ShoeStore.Application.Interfaces.Services.Shoes;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Models;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Services.Shoes;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public CategoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateAsync(categoryCreateDto, cancellationToken);

        var category = _mapper.Map<Category>(categoryCreateDto);

        var entity = _unitOfWork.Categories.Add(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(entity);
    }

    public async Task UpdateAsync(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateAsync(categoryUpdateDto, cancellationToken);

        var category = await GetCategoryAsync(categoryUpdateDto.CategoryId, cancellationToken);

        _mapper.Map(categoryUpdateDto, category);

        _unitOfWork.Categories.Update(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await GetCategoryAsync(id, cancellationToken);

        _unitOfWork.Categories.Remove(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<CategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await GetCategoryAsync(id, cancellationToken);

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<PagedList<CategoryDto>> GetAllAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken: cancellationToken);

        return _mapper.Map<PagedList<CategoryDto>>(categories);
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(
            cancellationToken: cancellationToken);

        return _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
    }

    private async Task<Category> GetCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Categories.GetSingleAsync(
            x => x.CategoryId == id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Category with id {id} not found");
    }
}