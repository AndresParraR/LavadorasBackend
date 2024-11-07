using Lavadoras.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Lavadoras.API.Requests;

public class CreateOperatorRequest
{
    [Required]
    [MaxLength(10)]
    [Display(Name = "Identification")]
    public string Identification { get; set; }

    [JsonIgnore]
    public int UserTypeId { get; init; } = (int)UserTypeEnum.Operator;

    [Required]
    public int RoleTypeId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
    [Required]
    public bool IsActive { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; }
}
