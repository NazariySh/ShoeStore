using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Application.DTOs.Shoes;
using ShoeStore.Application.Interfaces.Services;
using ShoeStore.Application.Interfaces.Services.Shoes;
using ShoeStore.Application.Utilities;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Models;
using ShoeStore.Domain.Repositories;
using System.Linq.Expressions;
using ShoeStore.Application.DTOs.Shoes.ShoeImages;
using ShoeStore.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ShoeStore.Application.Services.Shoes;

public class ShoeService : IShoeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;
    private readonly IValidationService _validationService;

    public ShoeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IImageService imageService,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    public async Task<Guid> CreateAsync(ShoeCreateDto shoeCreateDto, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateAsync(shoeCreateDto, cancellationToken);

        var shoe = _mapper.Map<Shoe>(shoeCreateDto);

        var entity = _unitOfWork.Shoes.Add(shoe);

        await UploadImagesAsync(shoeCreateDto.Images, entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ShoeId;
    }

    public async Task UpdateAsync(ShoeUpdateDto shoeUpdateDto, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateAsync(shoeUpdateDto, cancellationToken);

        var shoe = await _unitOfWork.Shoes.GetSingleAsync(
            x => x.ShoeId == shoeUpdateDto.ShoeId,
            include: x =>
                x.Include(s => s.ShoeImages),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Shoe with id {shoeUpdateDto.ShoeId} not found");

        _mapper.Map(shoeUpdateDto, shoe);

        _unitOfWork.Shoes.Update(shoe);

        await UploadImagesAsync(shoeUpdateDto.Images, shoe);

        var publicIds = new List<string>();

        foreach (var imageId in shoeUpdateDto.RemovedImageIds)
        {
            var shoeImage = shoe.ShoeImages.FirstOrDefault(i => i.ShoeImageId == imageId);

            if (shoeImage != null)
            {
                publicIds.Add(shoeImage.PublicId);
                shoe.ShoeImages.Remove(shoeImage);
            }
        }

        await DeleteImagesAsync(publicIds);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var shoe = await _unitOfWork.Shoes.GetSingleAsync(
            x => x.ShoeId == id,
            include: x =>
                x.Include(s => s.ShoeImages),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Shoe with id {id} not found");

        var publicIds = shoe.ShoeImages.Select(i => i.PublicId).ToList();

        _unitOfWork.Shoes.Remove(shoe);

        await DeleteImagesAsync(publicIds);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ShoeDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var shoe = await _unitOfWork.Shoes.GetSingleAsync(
            x => x.ShoeId == id,
            x => x
                .Include(s => s.Category)
                .Include(s => s.Brand)
                .Include(s => s.ShoeImages),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Shoe with id {id} not found");

        return _mapper.Map<ShoeDto>(shoe);
    }

    public async Task<PagedList<ShoeDto>> GetAllAsync(ShoeQuery query, CancellationToken cancellationToken = default)
    {
        var pagination = query.Pagination;
        var sort = query.Sort;

        var shoes = await _unitOfWork.Shoes.GetAllAsync(
            pagination.PageNumber,
            pagination.PageSize,
            query.Categories,
            query.Brands,
            GetSearchExpression(query),
            x => x
                .Include(s => s.Category)
                .Include(s => s.Brand)
                .Include(s => s.ShoeImages),
            GetSortExpression(sort.SortBy),
            sort.IsDescendingOrder,
            cancellationToken);

        return _mapper.Map<PagedList<ShoeDto>>(shoes);
    }

    private async Task UploadImagesAsync(ICollection<ShoeImageCreateDto> images, Shoe shoe)
    {
        if (images.Count == 0)
        {
            return;
        }

        var mainImage = images.FirstOrDefault(i => i.IsMain);

        foreach (var image in images)
        {
            var fileName = image.PublicId;
            var file = ConvertBase64ToFile(image.File, fileName);

            var imageUrl = await _imageService.UploadAsync(fileName, file);

            var shoeImage = new ShoeImage
            {
                PublicId = fileName,
                Url = imageUrl,
                IsMain = image == mainImage
            };

            shoe.ShoeImages.Add(shoeImage);
        }
    }

    private async Task DeleteImagesAsync(IEnumerable<string> publicIds)
    {
        foreach (var publicId in publicIds)
        {
            await _imageService.DeleteAsync(publicId);
        }
    }

    private static Expression<Func<Shoe, bool>>? GetSearchExpression(ShoeQuery query)
    {
        var searchTerm = query.Search.SearchTerm;

        if (string.IsNullOrEmpty(searchTerm))
        {
            return null;
        }

        return x =>
            x.Name.ToLower().Contains(searchTerm) ||
            x.Description.ToLower().Contains(searchTerm) ||
            x.Category.Name.ToLower().Contains(searchTerm) ||
            x.Brand.Name.ToLower().Contains(searchTerm);
    }

    private static Expression<Func<Shoe, object>> GetSortExpression(string? sortBy)
    {
        return SortExpression.BuildOrDefault<Shoe>(sortBy, x => x.CreatedAt);
    }

    private static FormFile ConvertBase64ToFile(string base64, string fileName)
    {
        var bytes = Convert.FromBase64String(base64.Split(',')[1]);
        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, bytes.Length, "file", fileName);
    }
}