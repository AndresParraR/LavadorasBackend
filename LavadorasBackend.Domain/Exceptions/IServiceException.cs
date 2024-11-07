using System.Net;

namespace Lavadoras.Domain.Exceptions;

public interface IServiceException
{
    public HttpStatusCode StatusCode { get; }
    public string Message { get; }
}
