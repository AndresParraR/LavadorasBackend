using System.Net;

namespace Lavadoras.Domain.Exceptions;

public class UnauthorizedException : Exception, IServiceException
{
    public UnauthorizedException(string message)
    {
        Message = message;
    }
    public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    public string Message { get; }
}
