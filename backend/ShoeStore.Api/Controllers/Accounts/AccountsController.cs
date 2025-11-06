using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Api.Extensions;
using ShoeStore.Application.DTOs.Accounts;
using ShoeStore.Application.Interfaces.Services.Users;

namespace ShoeStore.Api.Controllers.Accounts;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountsController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var userDto = await _userService.GetByIdAsync(User.GetId(), cancellationToken);

        return Ok(userDto);
    }

    [HttpDelete("profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfile(CancellationToken cancellationToken)
    {
        await _userService.DeleteAsync(User.GetId(), cancellationToken);

        return NoContent();
    }

    [HttpPut("address")]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateAddress(AddressDto addressDto, CancellationToken cancellationToken)
    {
        await _userService.UpdateAddressAsync(User.GetId(), addressDto, cancellationToken);

        return NoContent();
    }

    [HttpPatch("reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
        await _userService.ResetPassword(User.GetId(), changePasswordDto, cancellationToken);

        return NoContent();
    }
}