using FluentValidation;
using ShoeStore.Application.DTOs.Shoes;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Shoes;

public class ShoeUpdateDtoValidator : AbstractValidator<ShoeUpdateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoeUpdateDtoValidator(
        IValidator<ShoeCreateUpdateDto> validator,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        Include(validator);

        RuleFor(x => x)
            .MustAsync(BeUniqueName).WithMessage("Shoe with this name already exists.")
            .MustAsync(BeUniqueSku).WithMessage("Shoe with this SKU already exists.");
    }

    private async Task<bool> BeUniqueName(ShoeUpdateDto shoe, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Shoes.AnyAsync(
            x =>
                x.ShoeId != shoe.ShoeId &&
                x.Name == shoe.Name &&
                x.CategoryId == shoe.CategoryId &&
                x.BrandId == shoe.BrandId,
            cancellationToken);
    }

    private async Task<bool> BeUniqueSku(ShoeUpdateDto shoe, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Shoes.AnyAsync(
            x =>
                x.Sku == shoe.Sku &&
                x.ShoeId != shoe.ShoeId,
            cancellationToken);
    }
}