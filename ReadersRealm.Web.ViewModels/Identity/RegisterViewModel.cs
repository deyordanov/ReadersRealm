namespace ReadersRealm.Web.ViewModels.Identity;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class RegisterViewModel
{
    [BindProperty]
    public RegisterInputViewModel Input { get; set; } = null!;

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

    public string? ReturnUrl { get; set; }
}