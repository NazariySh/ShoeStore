using ShoeStore.Application.DTOs.Carts;

namespace ShoeStore.Application.Interfaces.Services.Carts;

public interface ICartService
{
    Task<ShoppingCartDto> CreateAsync(CancellationToken cancellationToken = default);
    Task<ShoppingCartDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddItemToCart(Guid id, CartItemDto item, int quantity, CancellationToken cancellationToken = default);
    Task RemoveItemFromCart(Guid id, Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task ClearItemsAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(ShoppingCartDto shoppingCart, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}