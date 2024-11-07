using Lavadoras.Domain.Entities;
using Lavadoras.Domain.Enumerators;
using Microsoft.EntityFrameworkCore;

namespace LavadorasBackend.Persistence;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> User => Set<User>();
    public DbSet<UserType> UserType => Set<UserType>();
    public DbSet<RoleType> RoleType => Set<RoleType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserType>().HasData(
            new UserType { Id = 1, Name = "Operator" },
            new UserType { Id = 2, Name = "Client" }
        );

        modelBuilder.Entity<RoleType>().HasData(
            new RoleType { Id = 1, Name = "Super Administrator" },
            new RoleType { Id = 2, Name = "Administrator" },
            new RoleType { Id = 3, Name = "Employee" }
        );

        modelBuilder.Entity<User>().HasData(
            new User {
                Id = 1,
                Identification = "1111111111",
                UserTypeId = (int)UserTypeEnum.Operator,
                RoleTypeId = (int)RoleTypeEnum.Super_Administrator,
                FirstName = "Super",
                LastName = "Admin",
                Email = "andrestupar0@gmail.com",
                IsActive = true
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}

