using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lavadoras.Domain.Entities;

namespace Lavadoras.Application.IRepositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> Get(string username);
    Task<User> GetByIdentification(string identification);
    Task<List<User>> GetAllOperators();
    Task<List<User>> CreateRange(List<User> users);
    Task<List<User>> UpdateRange(List<User> users);
    Task<User> ValidateToken(int id, string token);
    Task<User> Validate(string username, string password);
    Task<User> ValidateCode(string username, string codeVerification);
    Task<bool> ChangedPassword(User user);
}
