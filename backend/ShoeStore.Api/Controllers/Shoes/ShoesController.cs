using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Application.DTOs.Shoes;
using ShoeStore.Application.Interfaces.Services.Shoes;
using ShoeStore.Domain.Enums;
using ShoeStore.Domain.Models;

namespace ShoeStore.Api.Controllers.Shoes;

[Route("api/[controller]")]
[ApiController]
public class ShoesController : ControllerBase
{
    private readonly IShoeService _shoeService;

    public ShoesController(IShoeService shoeService)
    {
        _shoeService = shoeService ?? throw new ArgumentNullException(nameof(shoeService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<ShoeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ShoeQuery query, CancellationToken cancellationToken = default)
    {
        var shoes = await _shoeService.GetAllAsync(query, cancellationToken);

        return Ok(shoes);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ShoeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var shoe = await _shoeService.GetByIdAsync(id, cancellationToken);

        return Ok(shoe);
    }

    [HttpPost]
    [Authorize(Roles = nameof(RoleType.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] ShoeCreateDto shoeCreateDto, CancellationToken cancellationToken = default)
    {
        var shoeId = await _shoeService.CreateAsync(shoeCreateDto, cancellationToken);

        return Ok(shoeId);
    }

    [HttpPut]
    [Authorize(Roles = nameof(RoleType.Admin))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] ShoeUpdateDto shoe, CancellationToken cancellationToken = default)
    {
        await _shoeService.UpdateAsync(shoe, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(RoleType.Admin))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _shoeService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}