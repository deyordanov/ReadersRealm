namespace ReadersRealm.Web.ViewModels.Identity;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class RegisterViewModel
{
    public RegisterViewModel()
    {
        ExternalLogins = new List<AuthenticationScheme>();
    }

    [BindProperty]
    public RegisterInputViewModel Input { get; set; } = null!;

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string? ReturnUrl { get; set; }
}