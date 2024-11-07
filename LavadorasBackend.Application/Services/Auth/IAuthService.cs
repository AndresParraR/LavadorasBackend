using Lavadoras.Domain.Entities;

namespace Lavadoras.Application.Services.Auth;

public interface IAuthService
{
    Task<User> Login(string username, string password);
    Task<User> Login2FA(string username, string codeVerification);
    Task<User> Register(User user);
    Task<bool> ChangePassword(string username, string currentPassword, string newPassword);
    Task<bool> ForgotPassword(string username);
    Task<bool> SignOut(int currentUserId);
    Task<bool> ResendCodeVerification(string username);
    Task<User> ValidateToken(int id, string token);
    Task<User> RefreshToken(int id);
    Task<User> GenerateToken(User user);
}
