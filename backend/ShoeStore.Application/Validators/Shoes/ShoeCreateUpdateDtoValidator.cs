using FluentValidation;
using ShoeStore.Application.DTOs.Shoes;
using ShoeStore.Application.DTOs.Shoes.ShoeImages;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Shoes;

public class ShoeCreateUpdateDtoValidator : AbstractValidator<ShoeCreateUpdateDto>
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 500;
    public const decimal MaxPrice = 10_000;
    public const int MaxSkuLength = 50;
    public const int MaxImageCount = 5;

    private readonly IUnitOfWork _unitOfWork;

    public ShoeCreateUpdateDtoValidator(
        IValidator<ShoeImageCreateDto> validator,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(MaxNameLength).WithMessage($"Name must not exceed {MaxNameLength} characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(MaxDescriptionLength).WithMessage($"Description must not exceed {MaxDescriptionLength} characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.")
            .LessThanOrEqualTo(MaxPrice).WithMessage($"Price must not exceed {MaxPrice}.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("BrandId is required.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("Sku is required.")
            .MaximumLength(MaxSkuLength).WithMessage($"Sku must not exceed {MaxSkuLength} characters.");

        RuleFor(x => x.Stock)
            .GreaterThan(0).WithMessage("Stock must be greater than 0.");

        RuleFor(x => x.Images)
            .Must(x => x.Count <= MaxImageCount).WithMessage($"A maximum of {MaxImageCount} images are allowed.")
            .ForEach(x => x.SetValidator(validator));
    }
}