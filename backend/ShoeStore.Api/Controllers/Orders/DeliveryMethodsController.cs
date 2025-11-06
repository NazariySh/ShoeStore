using Microsoft.AspNetCore.Mvc;
using ShoeStore.Application.DTOs.Orders.DeliveryMethods;
using ShoeStore.Application.Interfaces.Services.Orders;

namespace ShoeStore.Api.Controllers.Orders;

[Route("api/orders/[controller]")]
[ApiController]
public class DeliveryMethodsController : ControllerBase
{
    private readonly IDeliveryMethodService _deliveryMethodService;

    public DeliveryMethodsController(IDeliveryMethodService deliveryMethodService)
    {
        _deliveryMethodService = deliveryMethodService ?? throw new ArgumentNullException(nameof(deliveryMethodService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var deliveryMethods = await _deliveryMethodService.GetAllAsync(cancellationToken);

        return Ok(deliveryMethods);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DeliveryMethodDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var deliveryMethod = await _deliveryMethodService.GetByIdAsync(id, cancellationToken);

        return Ok(deliveryMethod);
    }
}