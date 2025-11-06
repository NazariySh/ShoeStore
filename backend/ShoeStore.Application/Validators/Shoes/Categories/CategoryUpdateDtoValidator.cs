using FluentValidation;
using ShoeStore.Application.DTOs.Shoes.Categories;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Shoes.Categories;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryUpdateDtoValidator(
        IValidator<CategoryCreateUpdateDto> validator,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        Include(validator);

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x)
            .MustAsync(BeUniqueName).WithMessage("Category with this name already exists.");
    }

    private async Task<bool> BeUniqueName(CategoryUpdateDto category, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Categories.AnyAsync(
            x =>
                x.Name == category.Name &&
                x.CategoryId != category.CategoryId,
            cancellationToken);
    }
}