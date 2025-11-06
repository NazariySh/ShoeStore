using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Api.Extensions;
using ShoeStore.Application.DTOs.Orders;
using ShoeStore.Application.Interfaces.Services.Orders;
using ShoeStore.Domain.Enums;
using ShoeStore.Domain.Models;

namespace ShoeStore.Api.Controllers.Orders;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllOrdersForAccount([FromQuery] OrderQuery query, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(query, User.GetId(), cancellationToken);

        return Ok(orders);
    }

    [HttpGet("admin")]
    [Authorize(Roles = nameof(RoleType.Admin))]
    [ProducesResponseType(typeof(PagedList<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll([FromQuery] OrderQuery query, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(query, cancellationToken: cancellationToken);

        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(id, cancellationToken);

        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(OrderCreateDto order, CancellationToken cancellationToken)
    {
        var orderId = await _orderService.CreateAsync(order, User.GetId(), cancellationToken);

        return Ok(orderId);
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = nameof(RoleType.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromQuery] OrderStatus status,
        CancellationToken cancellationToken)
    {
        await _orderService.UpdateStatusAsync(id, status, cancellationToken);

        return Ok();
    }
}