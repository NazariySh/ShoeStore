using Microsoft.AspNetCore.Http;

namespace ShoeStore.Domain.Exceptions;

public class AlreadyExistsException : ApiException
{
    private const int DefaultStatusCode = StatusCodes.Status409Conflict;

    public AlreadyExistsException(string message)
        : base(DefaultStatusCode, message)
    {
    }
}