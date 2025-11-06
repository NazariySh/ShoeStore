using FluentValidation;
using ShoeStore.Application.DTOs.Shoes.Brands;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Shoes.Brands;

public class BrandUpdateDtoValidator : AbstractValidator<BrandUpdateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public BrandUpdateDtoValidator(
        IValidator<BrandCreateUpdateDto> validator,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        Include(validator);

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x)
            .MustAsync(BeUniqueName).WithMessage("Brand with this name already exists");
    }

    private async Task<bool> BeUniqueName(BrandUpdateDto brand, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Brands.AnyAsync(
            x =>
                x.Name == brand.Name &&
                x.BrandId != brand.BrandId,
            cancellationToken);
    }
}