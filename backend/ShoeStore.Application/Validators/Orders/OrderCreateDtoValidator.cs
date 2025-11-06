using FluentValidation;
using ShoeStore.Application.DTOs.Orders;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Orders;

public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreateDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        RuleFor(x => x.ShoppingCartId)
            .NotEmpty().WithMessage("Shopping cart is required.")
            .MustAsync(IsValidShoppingCart).WithMessage("Shopping cart does not exist.");

        RuleFor(x => x.DeliveryMethodId)
            .NotEmpty().WithMessage("Delivery method is required.")
            .MustAsync(IsValidDeliveryMethod).WithMessage("Delivery method does not exist.");
    }

    private Task<bool> IsValidShoppingCart(Guid shoppingCartId, CancellationToken cancellationToken)
    {
        return _unitOfWork.ShoppingCarts.AnyAsync(
            x => x.ShoppingCartId == shoppingCartId,
            cancellationToken: cancellationToken);
    }

    private Task<bool> IsValidDeliveryMethod(Guid deliveryMethodId, CancellationToken cancellationToken)
    {
        return _unitOfWork.DeliveryMethods.AnyAsync(
            x => x.DeliveryMethodId == deliveryMethodId,
            cancellationToken: cancellationToken);
    }
}