using FluentValidation;
using ShoeStore.Application.DTOs.Shoes.Categories;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Shoes.Categories;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryCreateDtoValidator(
        IValidator<CategoryCreateUpdateDto> validator,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        Include(validator);

        RuleFor(x => x)
            .MustAsync(BeUniqueName).WithMessage("Category with this name already exists.");
    }

    private async Task<bool> BeUniqueName(CategoryCreateDto category, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Categories.AnyAsync(
            x => x.Name == category.Name,
            cancellationToken);
    }
}