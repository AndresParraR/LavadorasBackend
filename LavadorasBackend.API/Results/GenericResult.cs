namespace Lavadoras.API.Results;

public class GenericResult<T>
{
    public bool Success { get; set; }
    public T Response { get; set; }
}