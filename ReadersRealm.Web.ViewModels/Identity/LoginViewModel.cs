using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Web.ViewModels.Identity;

public class LoginViewModel
{
    public LoginViewModel()
    {
        ExternalLogins = new List<AuthenticationScheme>();
    }

    [BindProperty]
    public LoginInputViewModel Input { get; set; } = null!;

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }
}