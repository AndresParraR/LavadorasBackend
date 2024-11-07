using Microsoft.EntityFrameworkCore;
using System;
using Lavadoras.Application.IRepositories;
using Lavadoras.Domain.Entities;
using LavadorasBackend.Persistence;
using Lavadoras.Domain.Enumerators;

namespace Lavadoras.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext) => _dataContext = dataContext;

    public async Task<bool> ChangedPassword(User user)
    {
        _dataContext.Update(user);
        var result = await _dataContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<User> Get(string username)
    {
        return await _dataContext.User.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<User> GetByIdentification(string identification)
    {
        return await _dataContext.User.FirstOrDefaultAsync(u => u.Identification == identification);
    }

    public Task<List<User>> GetAllOperators()
    {
        return _dataContext.User.Where(u => u.UserTypeId == (int)UserTypeEnum.Operator).Include(u => u.RoleType).ToListAsync();
    }

    public async Task<User> Create(User instance)
    {
        _dataContext.User.Add(instance);
        await _dataContext.SaveChangesAsync();
        return await _dataContext.User.FirstOrDefaultAsync(u => u.Id == instance.Id);
    }

    public async Task<User> Get(int id)
    {
        return await _dataContext.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> ValidateToken(int id, string token)
    {
        return await _dataContext.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && u.Token == token);
    }

    public async Task<User> Update(User instance)
    {
        _dataContext.Update(instance);
        await _dataContext.SaveChangesAsync();
        return await _dataContext.User.FirstOrDefaultAsync(u => u.Id == instance.Id);
    }

    public async Task<List<User>> UpdateRange(List<User> instance)
    {
        _dataContext.UpdateRange(instance);
        await _dataContext.SaveChangesAsync();
        return instance;
    }

    public async Task<User> Validate(string username, string password)
    {
        return await _dataContext.User.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
    }

    public async Task<User> ValidateCode(string username, string codeVerification)
    {
        return await _dataContext.User.FirstOrDefaultAsync(u => u.UserName == username && u.CodeVerification == codeVerification);
    }

    public Task<List<User>> GetAll()
    {
        return _dataContext.User.ToListAsync();
    }

    public async Task<List<User>> CreateRange(List<User> users)
    {
        _dataContext.User.AddRange(users);
        await _dataContext.SaveChangesAsync();
        return users;
    }
}
