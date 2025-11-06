using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Application.DTOs.Carts;
using ShoeStore.Application.Interfaces.Services.Carts;
using ShoeStore.Domain.Entities.Carts;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Services.Carts;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CartService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ShoppingCartDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await _unitOfWork.ShoppingCarts.GetSingleAsync(
            x => x.ShoppingCartId == id,
            include: x => x
                .Include(c => c.DeliveryMethod)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.ShoeImages),
            cancellationToken: cancellationToken);

        if (cart is null)
        {
            _unitOfWork.ShoppingCarts.Add(new ShoppingCart
            {
                ShoppingCartId = id,
            });

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ShoppingCartDto
            {
                ShoppingCartId = id,
            };
        }
        
        return _mapper.Map<ShoppingCartDto>(cart);
    }

    public async Task AddItemToCart(Guid id, CartItemDto item, int quantity, CancellationToken cancellationToken = default)
    {
        var cartItem = await _unitOfWork.CartItems.GetSingleAsync(
            x => x.ShoppingCartId == id && x.ProductId == item.ProductId,
            cancellationToken: cancellationToken);

        if (cartItem is null)
        {
            cartItem = new CartItem
            {
                ShoppingCartId = id,
                ProductId = item.ProductId,
                Quantity = quantity,
            };
            _unitOfWork.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
            _unitOfWork.CartItems.Update(cartItem);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveItemFromCart(Guid id, Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var cartItem = await _unitOfWork.CartItems.GetSingleAsync(
            x => x.ShoppingCartId == id && x.ProductId == productId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Cart item with product id {productId} not found in cart {id}.");

        if (cartItem.Quantity > quantity)
        {
            cartItem.Quantity -= quantity;
            _unitOfWork.CartItems.Update(cartItem);
        }
        else
        {
            _unitOfWork.CartItems.Remove(cartItem);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ClearItemsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await _unitOfWork.ShoppingCarts.GetSingleAsync(
            x => x.ShoppingCartId == id,
            include: x => x
                .Include(c => c.CartItems),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Shopping cart with id {id} not found.");

        cart.CartItems.Clear();
        _unitOfWork.ShoppingCarts.Update(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ShoppingCartDto> CreateAsync(CancellationToken cancellationToken = default)
    {
        var cart = new ShoppingCart
        {
            ShoppingCartId = Guid.NewGuid(),
        };

        _unitOfWork.ShoppingCarts.Add(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ShoppingCartDto
        {
            ShoppingCartId = cart.ShoppingCartId,
        };
    }

    public async Task UpdateAsync(ShoppingCartDto shoppingCart, CancellationToken cancellationToken = default)
    {
        var cart = await _unitOfWork.ShoppingCarts.GetSingleAsync(
            x => x.ShoppingCartId == shoppingCart.ShoppingCartId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Shopping cart with id {shoppingCart.ShoppingCartId} not found.");

        cart.DeliveryMethodId = shoppingCart.DeliveryMethod?.DeliveryMethodId;
        cart.CartItems.Clear();

        foreach (var item in shoppingCart.Items)
        {
            cart.CartItems.Add(new CartItem
            {
                ShoppingCartId = cart.ShoppingCartId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            });
        }

        _unitOfWork.ShoppingCarts.Update(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await _unitOfWork.ShoppingCarts.GetSingleAsync(
            x => x.ShoppingCartId == id,
            include: x => x
                .Include(c => c.CartItems),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Shopping cart with id {id} not found.");

        _unitOfWork.ShoppingCarts.Remove(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}