using System.Net;

namespace Lavadoras.Domain.Exceptions;

public class NotFoundException : Exception, IServiceException
{
    public NotFoundException(string message)
    {
        Message = message;
    }
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public string Message { get; }
}
