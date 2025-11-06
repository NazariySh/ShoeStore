using Microsoft.AspNetCore.Mvc;
using ShoeStore.Application.DTOs.Carts;
using ShoeStore.Application.Interfaces.Services.Carts;

namespace ShoeStore.Api.Controllers.Carts;

[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartsController(ICartService cartService)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ShoppingCartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var cart = await _cartService.GetAsync(id, cancellationToken);

        return Ok(cart);
    }

    [HttpPost("{id:guid}/items")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddItemToCart(Guid id, [FromBody] CartItemDto item, [FromQuery] int quantity, CancellationToken cancellationToken)
    {
        await _cartService.AddItemToCart(id, item, quantity, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}/items/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItemFromCart(Guid id, Guid productId, [FromQuery] int quantity, CancellationToken cancellationToken)
    {
        await _cartService.RemoveItemFromCart(id, productId, quantity, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}/items")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ClearItems(Guid id, CancellationToken cancellationToken)
    {
        await _cartService.ClearItemsAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCartDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create()
    {
        var cart = await _cartService.CreateAsync();

        return CreatedAtAction(nameof(GetById), new { id = cart.ShoppingCartId }, cart);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(ShoppingCartDto cart, CancellationToken cancellationToken)
    {
        await _cartService.UpdateAsync(cart, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _cartService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}