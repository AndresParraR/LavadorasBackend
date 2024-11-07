
using Lavadoras.Application.Common;
using Lavadoras.Application.IRepositories;
using Lavadoras.Domain.Crosscuting;
using Lavadoras.Domain.Entities;
using Lavadoras.Domain.Exceptions;

namespace Lavadoras.Application.Services.Operator;

public class OperatorService : IOperatorService
{

    private readonly IUserRepository _userRepository;
    private readonly IEmail _email;

    public OperatorService(IUserRepository userRepository, IEmail email)
    {
        _userRepository = userRepository;
        _email = email;
    }

    public async Task<User> Create(User user, int currentUserId)
    {
        var password = RandomPasswordGenerator.GeneratePassword(true, true, true, false, 8);
        user.Password = Encryptor.Sha256Hash(password);
        var newUser = await _userRepository.Create(user);
        _email.SendCredentials(newUser.Email, newUser.UserName ?? "No Username", newUser.FirstName, password);
        return newUser;
    }

    public Task<List<User>> GetAll()
    {
        return _userRepository.GetAllOperators();
    }

    public async Task<User> Update(int id,User user, int currentUserId)
    {
        var currentUser = await _userRepository.Get(id);
        if (currentUser == null)
        {
            throw new ConflictException("User not found");
        }
        currentUser.Identification = user.Identification;
        currentUser.FirstName = user.FirstName;
        currentUser.LastName = user.LastName;
        currentUser.Email = user.Email;
        currentUser.IsActive = user.IsActive;
        currentUser.RoleTypeId = user.RoleTypeId;
        currentUser.UserName = user.UserName;
        return await _userRepository.Update(currentUser);
    }
}
