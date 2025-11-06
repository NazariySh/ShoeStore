using FluentValidation;
using ShoeStore.Application.DTOs.Shoes.Categories;

namespace ShoeStore.Application.Validators.Shoes.Categories;

public class CategoryCreateUpdateDtoValidator : AbstractValidator<CategoryCreateUpdateDto>
{
    public const int NameMaxLength = 50;

    public CategoryCreateUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(NameMaxLength).WithMessage($"Name must not exceed {NameMaxLength} characters.");
    }
}