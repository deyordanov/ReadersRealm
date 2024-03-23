using System.ComponentModel.DataAnnotations;

namespace ReadersRealm.Web.ViewModels.Identity;

public class LoginInputViewModel
{
    [EmailAddress]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}