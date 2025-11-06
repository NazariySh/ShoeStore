using System.Security.Claims;

namespace ShoeStore.Api.Extensions;

public static class ClaimPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal principal)
    {
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(id, out var result)
            ? result
            : throw new UnauthorizedAccessException("User Id is unavailable");
    }
}