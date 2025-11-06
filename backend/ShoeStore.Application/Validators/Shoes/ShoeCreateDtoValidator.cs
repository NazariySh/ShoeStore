using FluentValidation;
using ShoeStore.Application.DTOs.Shoes;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Shoes;

public class ShoeCreateDtoValidator : AbstractValidator<ShoeCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoeCreateDtoValidator(
        IValidator<ShoeCreateUpdateDto> validator,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        Include(validator);

        RuleFor(x => x)
            .MustAsync(BeUniqueName)
            .WithMessage("Shoe with this name already exists.");

        RuleFor(x => x.Sku)
            .MustAsync(BeUniqueSku).WithMessage("Shoe with this sku already exists.");
    }

    private async Task<bool> BeUniqueName(ShoeCreateDto shoe, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Shoes.AnyAsync(
            x =>
                x.Name == shoe.Name &&
                x.CategoryId == shoe.CategoryId &&
                x.BrandId == shoe.BrandId,
            cancellationToken);
    }

    private async Task<bool> BeUniqueSku(string sku, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Shoes.AnyAsync(
            x => x.Sku == sku,
            cancellationToken);
    }
}