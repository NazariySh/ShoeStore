using FluentValidation;
using ShoeStore.Application.DTOs.Shoes.Brands;

namespace ShoeStore.Application.Validators.Shoes.Brands;

public class BrandCreateUpdateDtoValidator : AbstractValidator<BrandCreateUpdateDto>
{
    public const int NameMaxLength = 50;

    public BrandCreateUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(NameMaxLength).WithMessage($"Name must not exceed {NameMaxLength} characters.");
    }
}