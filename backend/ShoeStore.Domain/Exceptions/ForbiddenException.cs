using Microsoft.AspNetCore.Http;

namespace ShoeStore.Domain.Exceptions;

public class ForbiddenException : ApiException
{
    private const int DefaultStatusCode = StatusCodes.Status403Forbidden;

    public ForbiddenException(string message)
        : base(DefaultStatusCode, message)
    {
    }
}