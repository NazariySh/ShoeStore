using FluentValidation;
using ShoeStore.Application.DTOs.Shoes.ShoeImages;

namespace ShoeStore.Application.Validators.Shoes.ShoeImages;

public class ShoeImageCreateDtoValidator : AbstractValidator<ShoeImageCreateDto>
{
    public const int PublicIdMaxLength = 50;

    public ShoeImageCreateDtoValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(file => file.Length > 0).WithMessage("File must not be empty.");

        RuleFor(x => x.PublicId)
            .MaximumLength(PublicIdMaxLength).WithMessage($"PublicId must not exceed {PublicIdMaxLength} characters.")
            .When(x => !string.IsNullOrEmpty(x.PublicId));

        RuleFor(x => x.IsMain)
            .NotNull().WithMessage("IsMain is required.");
    }
}