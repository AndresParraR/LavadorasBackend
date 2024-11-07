using System.Net;

namespace Lavadoras.Domain.Exceptions;

public class ConflictException : Exception, IServiceException
{
    public ConflictException(string message)
    {
        Message = message;
    }
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string Message { get; }
}
