using FluentValidation;
using ShoeStore.Application.DTOs.Shoes.Brands;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Shoes.Brands;

public class BrandCreateDtoValidator : AbstractValidator<BrandCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public BrandCreateDtoValidator(
        IValidator<BrandCreateUpdateDto> validator,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        Include(validator);

        RuleFor(x => x)
            .MustAsync(BeUniqueName).WithMessage("Brand with this name already exists");
    }

    private async Task<bool> BeUniqueName(BrandCreateDto brand, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Brands.AnyAsync(
            x => x.Name == brand.Name,
            cancellationToken);
    }
}