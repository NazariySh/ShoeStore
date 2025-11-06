using Microsoft.AspNetCore.Http;

namespace ShoeStore.Domain.Exceptions;

public class NotFoundException : ApiException
{
    private const int DefaultStatusCode = StatusCodes.Status404NotFound;

    public NotFoundException(string message)
        : base(DefaultStatusCode, message)
    {
    }
}