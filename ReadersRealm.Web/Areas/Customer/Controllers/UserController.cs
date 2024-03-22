namespace ReadersRealm.Web.Areas.Customer.Controllers;

using System.Text;
using System.Text.Encodings.Web;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using ReadersRealm.Web.ViewModels.Identity;
using Services.Data.CompanyServices.Contracts;
using Services.Data.IdentityServices.Contracts;
using ViewModels.Company;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.RedirectConstants;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.SendGridSettingsConstants;
using static Common.Constants.Constants.SessionKeysConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.Constants.UserConstants;
using static Common.Constants.ValidationMessageConstants.CompanyValidationMessages;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

[Area(Customer)]
public class UserController : BaseController
{
    private readonly ICompanyRetrievalService _companyRetrievalService;
    private readonly IIdentityRetrievalService _identityRetrievalService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IEmailSender _emailSender;

    public UserController(
        ICompanyRetrievalService companyRetrievalService,
        IIdentityRetrievalService identityRetrievalService,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole<Guid>> roleManager, 
        UserManager<ApplicationUser> userManager, 
        IUserStore<ApplicationUser> userStore,
        IEmailSender emailSender)
    {
        this._companyRetrievalService = companyRetrievalService;
        this._identityRetrievalService = identityRetrievalService;
        this._signInManager = signInManager;
        this._roleManager = roleManager;
        this._userManager = userManager;
        this._userStore = userStore;
        this._emailSender = emailSender;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl)
    {
        LoginViewModel loginModel = this
            ._identityRetrievalService
            .GetLoginModelForLogin();

        if (!string.IsNullOrEmpty(loginModel.ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, loginModel.ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        loginModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        loginModel.ReturnUrl = returnUrl;

        return View(loginModel);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel loginModel)
    {
        loginModel.ReturnUrl ??= Url.Content("~/");

        loginModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            SignInResult result = await _signInManager.PasswordSignInAsync(
                loginModel.Input.Email,
                loginModel.Input.Password,
                loginModel.Input.RememberMe,
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return LocalRedirect(loginModel.ReturnUrl);
            }

            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return this.View(loginModel);
        }

        return View(loginModel);
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string? returnUrl)
    {
        HttpContext.Session.Remove(ShoppingCartSessionKey);

        await _signInManager.SignOutAsync();
        if (returnUrl != null)
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(IndexActionName, HomeControllerName);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string? returnUrl)
    {
        RegisterViewModel registerModel = this
            ._identityRetrievalService
            .GetRegisterModelForRegister();

        List<AllCompaniesListViewModel> companies = await this
            ._companyRetrievalService
            .GetAllListAsync();

        RegisterInputViewModel registerInputModel = new RegisterInputViewModel()
        {
            Email = string.Empty,
            Password = string.Empty,
            ConfirmPassword = string.Empty,
            FirstName = string.Empty,
            LastName = string.Empty,
            Roles = _roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Name)),
            Companies = companies.Select(c => new SelectListItem(c.Name, c.Id.ToString())),
        };

        registerModel.ReturnUrl ??= Url.Content("~/");
        registerModel.ExternalLogins = (await this
            ._signInManager
            .GetExternalAuthenticationSchemesAsync())
            .ToList();

        return View(registerModel);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel registerModel)
    {
        registerModel.ReturnUrl ??= Url.Content("~/");

        registerModel.ExternalLogins = (await this
            ._signInManager
            .GetExternalAuthenticationSchemesAsync()).ToList();

        bool companyExists = registerModel.Input.CompanyId == null || await this
            ._companyRetrievalService
            .CompanyExistsAsync((Guid)registerModel
                .Input
                .CompanyId);

        if (!companyExists)
        {
            ModelState.AddModelError(nameof(registerModel.Input.CompanyId), CompanyDoesNotExistMessage);
        }

        if (!ModelState.IsValid)
        {
            List<AllCompaniesListViewModel> companies = await this
                ._companyRetrievalService
                .GetAllListAsync();

            registerModel.Input.Roles = this
                ._roleManager
                .Roles
                .Select(r => new SelectListItem(r.Name, r.Name));
            registerModel.Input.Companies = companies
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()));

            return View(registerModel);
        }

        ApplicationUser user = this.CreateUser();

        await this
            ._userStore
            .SetUserNameAsync(user, registerModel.Input.Email, CancellationToken.None);
        await this
            .GetEmailStore()
            .SetEmailAsync(user, registerModel.Input.Email, CancellationToken.None);

        user.FirstName = registerModel.Input.FirstName;
        user.LastName = registerModel.Input.LastName;
        user.StreetAddress = registerModel.Input.StreetAddress;
        user.City = registerModel.Input.City;
        user.State = registerModel.Input.State;
        user.PostalCode = registerModel.Input.PostalCode;
        user.PhoneNumber = registerModel.Input.PhoneNumber;

        if (registerModel.Input.Role == CompanyRole)
        {
            user.CompanyId = registerModel.Input.CompanyId;
        }

        var result = await this
            ._userManager
            .CreateAsync(user, registerModel.Input.Password);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(registerModel.Input.Role))
            {
                await this
                    ._userManager
                    .AddToRoleAsync(user, registerModel.Input.Role);
            }
            else
            {
                await this
                    ._userManager
                    .AddToRoleAsync(user, CustomerRole);
            }

            string userId = await this
                ._userManager
                .GetUserIdAsync(user);
            string code = await this
                ._userManager
                .GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,

                values: new { area = "Identity", userId = userId, code = code, returnUrl = registerModel.ReturnUrl },
                protocol: Request.Scheme)!;

            string mainPageUrl = Url.Page(
                "/Home/Index",
            pageHandler: null,
                values: new { area = "Customer" },
                protocol: Request.Scheme)!;

            await this
                ._emailSender
                .SendEmailAsync(registerModel.Input.Email,
                EmailConfirmationSubject,
                this.BuildEmailMessage(callbackUrl, registerModel.Input));

            if (this._userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = registerModel.Input.Email, returnUrl = registerModel.ReturnUrl });
            }
            else
            {
                if (!User.IsInRole(AdminRole))
                {
                    await this
                        ._signInManager
                        .SignInAsync(user, isPersistent: false);
                }
                else
                {
                    TempData[Success] = UserHasSuccessfullyBeenCreated;
                }

                return LocalRedirect(registerModel.ReturnUrl);
            }
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View();
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                                                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!this._userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<ApplicationUser>)this._userStore;
    }

    private string BuildEmailMessage(string callbackUrl, RegisterInputViewModel registerInputModel)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(string.Format(EmailConfirmationHeaderMessage, registerInputModel.FirstName));
        sb.AppendLine(string.Format(EmailConfirmationBodyMessage, HtmlEncoder.Default.Encode(callbackUrl)));
        sb.AppendLine(EmailConfirmationFooterMessage);

        return sb.ToString().TrimEnd();
    }
}