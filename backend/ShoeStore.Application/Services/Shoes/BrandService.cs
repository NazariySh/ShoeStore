using AutoMapper;
using ShoeStore.Application.DTOs;
using ShoeStore.Application.DTOs.Shoes.Brands;
using ShoeStore.Application.Interfaces.Services;
using ShoeStore.Application.Interfaces.Services.Shoes;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Models;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Services.Shoes;

public class BrandService : IBrandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public BrandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    public async Task<BrandDto> CreateAsync(BrandCreateDto brandCreateDto, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateAsync(brandCreateDto, cancellationToken);

        var brand = _mapper.Map<Brand>(brandCreateDto);

        var entity = _unitOfWork.Brands.Add(brand);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BrandDto>(entity);
    }

    public async Task UpdateAsync(BrandUpdateDto brandUpdateDto, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateAsync(brandUpdateDto, cancellationToken);

        var brand = await GetBrandAsync(brandUpdateDto.BrandId, cancellationToken);

        _mapper.Map(brandUpdateDto, brand);

        _unitOfWork.Brands.Update(brand);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var brand = await GetBrandAsync(id, cancellationToken);

        _unitOfWork.Brands.Remove(brand);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<BrandDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var brand = await GetBrandAsync(id, cancellationToken);

        return _mapper.Map<BrandDto>(brand);
    }

    public async Task<PagedList<BrandDto>> GetAllAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        var brands = await _unitOfWork.Brands.GetAllAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken: cancellationToken);

        return _mapper.Map<PagedList<BrandDto>>(brands);
    }

    public async Task<IReadOnlyList<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var brands = await _unitOfWork.Brands.GetAllAsync(
            cancellationToken: cancellationToken);

        return _mapper.Map<IReadOnlyList<BrandDto>>(brands);
    }

    private async Task<Brand> GetBrandAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Brands.GetSingleAsync(
            x => x.BrandId == id,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Brand with id {id} not found");
    }
}