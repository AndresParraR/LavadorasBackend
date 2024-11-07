using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lavadoras.API.Requests;

public class LoginRequest
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
}
