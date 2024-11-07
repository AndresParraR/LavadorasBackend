
using Lavadoras.Domain.Entities;

namespace Lavadoras.Application.Services.Operator;

public interface IOperatorService
{
    Task<User> Create(User user, int currentUserId);
    Task<List<User>> GetAll();
    Task<User> Update(int id, User user, int currentUserId);
}
