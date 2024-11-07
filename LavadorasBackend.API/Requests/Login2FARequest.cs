using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lavadoras.API.Requests;

public class Login2FARequest
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [Display(Name = "CodeVerification")]
    public string CodeVerification { get; set; }
}
