using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Web.ViewModels.Identity;

public class LoginViewModel
{
    [BindProperty]
    public LoginInputViewModel Input { get; set; } = null!;

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }
}