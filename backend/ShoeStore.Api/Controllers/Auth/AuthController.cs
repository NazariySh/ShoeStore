using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Api.Extensions;
using ShoeStore.Application.DTOs.Auth;
using ShoeStore.Application.Interfaces.Services.Auth;
using ShoeStore.Domain.Enums;

namespace ShoeStore.Api.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var token = await _authService.LoginAsync(loginDto, cancellationToken);

        return Ok(token);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        await _authService.RegisterAsync(registerDto, RoleType.Customer, cancellationToken);

        return NoContent();
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshToken(string accessToken, CancellationToken cancellationToken)
    {
        var token = await _authService.RefreshTokenAsync(accessToken, cancellationToken);

        return Ok(token);
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(User.GetId(), cancellationToken);

        return NoContent();
    }

    [HttpPost("register/{roleType}")]
    [Authorize(Roles = nameof(RoleType.Admin))]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegisterWithRole(RoleType roleType, RegisterDto registerDto, CancellationToken cancellationToken)
    {
        await _authService.RegisterAsync(registerDto, roleType, cancellationToken);

        return NoContent();
    }
}