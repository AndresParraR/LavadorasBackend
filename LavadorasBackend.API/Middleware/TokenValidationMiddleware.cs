using Lavadoras.Application.Common.JWT;
using Lavadoras.Application.Services.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Lavadoras.API.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public TokenValidationMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        string headerToken = context.Request.Headers["Authorization"];
        if (headerToken != null && !context.Request.Path.ToString().Contains("hangfire") && !context.Request.Path.ToString().Contains("auth-token"))
        {
            if (headerToken == null)
            {
                new SecurityTokenValidationException();
                await _next(context);
                return;
            }

            var token = headerToken.Substring(7);
            if (token.Length < 70)
            {
                await _next(context);
                return;
            }
            var jwt = context.RequestServices.GetService<IJwtToken>();
            var authService = context.RequestServices.GetService<IAuthService>();
            var jToken = jwt.decodeJwtToken(token);
            var currentUserId = int.Parse(jToken.Claims.First(claim => claim.Type == "sub").Value);

            var user = await authService.ValidateToken(currentUserId, token);
            if (user == null) new SecurityTokenValidationException();
        }

        await _next(context);
    }
}
