using System.ComponentModel.DataAnnotations.Schema;

namespace Lavadoras.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Identification { get; set; }
    public int UserTypeId { get; set; }
    public int? RoleTypeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string? UserName { get; set; }
    public bool IsActive { get; set; } = true;
    public string? CodeVerification { get; set; }
    public string? Token { get; set; }
    public string? SecretKey { get; set; }
    public UserType UserType { get; set; }
    public RoleType? RoleType { get; set; }
}
