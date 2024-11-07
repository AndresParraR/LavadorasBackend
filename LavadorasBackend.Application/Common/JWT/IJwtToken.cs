using Lavadoras.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Lavadoras.Application.Common.JWT;

public interface IJwtToken
{
    string GenerateJwtToken(User user, DateTime expDate);
    public JwtSecurityToken decodeJwtToken(string token);
}
