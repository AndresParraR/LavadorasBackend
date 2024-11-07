using System.Net;
using System.Text.Json;
using Lavadoras.Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace Lavadoras.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.StatusCode;
            var response = _env.IsDevelopment()
                ? new ErrorException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ErrorException(context.Response.StatusCode, ex.Message);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        catch (ConflictException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.StatusCode;
            var response = _env.IsDevelopment()
                ? new ErrorException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ErrorException(context.Response.StatusCode, ex.Message);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        catch (UnauthorizedException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.StatusCode;
            var response = _env.IsDevelopment()
                ? new ErrorException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ErrorException(context.Response.StatusCode, ex.Message);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        catch (SecurityTokenValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var response = _env.IsDevelopment()
                ? new ErrorException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ErrorException(context.Response.StatusCode, ex.Message);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ErrorException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ErrorException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
