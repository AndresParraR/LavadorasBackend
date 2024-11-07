using Lavadoras.Application.Common;
using Lavadoras.Application.Common.JWT;
using Lavadoras.Application.IRepositories;
using Lavadoras.Domain.Crosscuting;
using Lavadoras.Domain.Entities;
using Lavadoras.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavadoras.Application.Services.Auth;

public class AuthService : IAuthService
{

    private readonly IUserRepository _userRepository;
    private readonly IJwtToken _jwtToken;
    private readonly IEmail _email;

    public AuthService(IUserRepository userRepository, IEmail email, IJwtToken jwtToken)
    {
        _userRepository = userRepository;
        _email = email;
        _jwtToken = jwtToken;
    }

    public async Task<bool> ChangePassword(string username, string currentPassword, string newPassword)
    {
        username = username.Trim().ToLower();
        currentPassword = Encryptor.Sha256Hash(currentPassword);
        var user = await _userRepository.Validate(username, currentPassword);
        if (user == null)
        {
            throw new ConflictException("Credenciales Incorrectas");
        }

        user.Password = Encryptor.Sha256Hash(newPassword);
        var passwordChanged = await _userRepository.ChangedPassword(user);
        if (passwordChanged)
        {
            _email.SendChangePasswordConfirmation(user.Email, user.FirstName);
            //user.CodeVerification = null;
            //user.Token = null;
        }
        return passwordChanged;
    }

    public async Task<bool> ForgotPassword(string username)
    {
        username = username.Trim().ToLower();
        var user = await _userRepository.Get(username);
        if(user == null)
        {
            throw new ConflictException("User not found");
        }
        var password = RandomPasswordGenerator.GeneratePassword(true, true, true, false, 8);
        user.Password = Encryptor.Sha256Hash(password);
        user.CodeVerification = null;
        user.SecretKey = null;
        user.Token = null;
        await _userRepository.Update(user);
        _email.SendCredentials(user.Email ,user.UserName ?? "No Username", user.FirstName, password);
        return true;
    }

    public async Task<bool> ResendCodeVerification(string username)
    {
        username = username.Trim().ToLower();
        var user = await _userRepository.Get(username);
        if (user == null)
        {
            throw new ConflictException("Credenciales Incorrectas");
        }

        var secretKey = Totp.SecretKey();

        user.SecretKey = secretKey;

        var codeVerification = Totp.GenerateCode(secretKey);

        user.CodeVerification = Encryptor.Sha256Hash(codeVerification);

        var userWithCode = await _userRepository.Update(user);

        _email.SendCodeVerification(userWithCode.Email, userWithCode.FirstName, codeVerification);

        return true;
    }

    public async Task<User> GenerateToken(User user)
    {
        var expDate = DateTime.Now.AddDays(5);
        var token = _jwtToken.GenerateJwtToken(user, expDate);
        user.Token = token;
        var userWithToken = await _userRepository.Update(user);
        return userWithToken;
    }

    public async Task<User> ValidateToken(int id, string token)
    {
        var user = await _userRepository.ValidateToken(id, token);
        return user;
    }

    public async Task<User> Login(string username, string password)
    {
        username = username.Trim().ToLower();
        password = Encryptor.Sha256Hash(password);
        var user = await _userRepository.Validate(username, password);
        
        if (user == null)
        {
            throw new ConflictException("Credenciales Incorrectas");
        }

        if (!user.IsActive)
        {
            throw new ConflictException("Usuario Inactivo");
        }

        var secretKey = Totp.SecretKey();

        user.SecretKey = secretKey;

        var codeVerification = Totp.GenerateCode(secretKey);

        user.CodeVerification = Encryptor.Sha256Hash(codeVerification);

        var userWithCode = await _userRepository.Update(user);

        _email.SendCodeVerification(userWithCode.Email, userWithCode.FirstName, codeVerification);
        
        return userWithCode;
    }

    public async Task<User> Login2FA(string username, string codeVerification)
    {
        username = username.Trim().ToLower();
        var codeVerificationHashed = Encryptor.Sha256Hash(codeVerification);
        var user = await _userRepository.ValidateCode(username, codeVerificationHashed);
        if (user == null)
        {
            throw new ConflictException("Codigo Incorrecto");
        }

        if (!user.IsActive)
        {
            throw new ConflictException("Usuario Inactivo");
        }

        var codeVerified = Totp.VerifyCode(user.SecretKey, codeVerification);

        if (!codeVerified) throw new ConflictException("Code already expired");

        return user;
    }

    public Task<User> RefreshToken(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Register(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SignOut(int currentUserId)
    {
        var user = await _userRepository.Get(currentUserId);
        if (user == null)
        {
            throw new ConflictException("Dont exist");
        }

        user.Token = null;
        user.CodeVerification = null;
        user.SecretKey = null;

        var userUpdated = await _userRepository.Update(user);

        return true;
    }
}
