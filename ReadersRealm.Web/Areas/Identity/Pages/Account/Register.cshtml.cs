// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace ReadersRealm.Web.Areas.Identity.Pages.Account;

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Services.Data.CompanyServices.Contracts;
using ViewModels.Company;
using static Common.Constants.Constants.RolesConstants;
using static Common.Constants.Constants.SendGridSettingsConstants;
using static Common.Constants.Constants.SharedConstants;
using static Common.Constants.Constants.UserConstants;
using static Common.Constants.ValidationConstants.RegisterModelValidation;
using static Common.Constants.ValidationMessageConstants.CompanyValidationMessages;
using static Common.Constants.ValidationMessageConstants.RegisterModelValidationMessages;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IEmailSender _emailSender;
    private readonly ICompanyRetrievalService _companyRetrievalService;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender, 
        ICompanyRetrievalService companyRetrievalService)
    {
        this._userManager = userManager;
        this._userStore = userStore;
        this._emailStore = GetEmailStore();
        this._signInManager = signInManager;
        this._roleManager = roleManager;
        this._logger = logger;
        this._emailSender = emailSender;
        this._companyRetrievalService = companyRetrievalService;
    }   

    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public string ReturnUrl { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(RegisterModelFirstNameMaxLength,
            MinimumLength = RegisterModelFirstNameMinLength,
            ErrorMessage = RegisterModelFirstNameLengthMessage)]
        public required string FirstName { get; set; }

        [Required]
        [Display(Name  = "Last Name")]
        [StringLength(RegisterModelLastNameMaxLength,
            MinimumLength = RegisterModelLastNameMinLength,
            ErrorMessage = RegisterModelLastNameLengthMessage)]
        public required string LastName { get; set; }

        [Display(Name = "Street Address")]
        [StringLength(RegisterModelStreetAddressMaxLength, 
            MinimumLength = RegisterModelStreetAddressMinLength,
            ErrorMessage = RegisterModelStreetAddressLengthMessage)]
        public string? StreetAddress { get; set; }

        [StringLength(RegisterModelCityMaxLength,
            MinimumLength = RegisterModelCityMinLength,
            ErrorMessage = RegisterModelCityLengthMessage)]
        public string? City { get; set; }

        [StringLength(RegisterModelStateMaxLength,
            MinimumLength = RegisterModelStateMinLength,
            ErrorMessage = RegisterModelStateLengthMessage)]
        public string? State { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(RegisterModelPostalCodeMaxLength,
            MinimumLength = RegisterModelPostalCodeMinLength,
            ErrorMessage = RegisterModelPostalCodeLengthMessage)]
        public string? PostalCode { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(RegisterModelPhoneNumberMaxLength,
            MinimumLength = RegisterModelPhoneNumberMinLength,
            ErrorMessage = RegisterModelPhoneNumberLengthMessage)]
        public string? PhoneNumber { get; set; }

        public string? Role { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Roles { get; set; }

        public Guid? CompanyId { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }
    }


    public async Task OnGetAsync(string returnUrl = null)
    {
        List<AllCompaniesListViewModel> companies = await this
            ._companyRetrievalService
            .GetAllListAsync();

        Input = new InputModel()
        {
            Email = string.Empty,
            Password = string.Empty,
            ConfirmPassword = string.Empty,
            FirstName = string.Empty,
            LastName = string.Empty,
            Roles = _roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Name)),
            Companies = companies.Select(c => new SelectListItem(c.Name, c.Id.ToString())),
        };

        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    { 
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        bool companyExists = this.Input.CompanyId == null || 
                             await this
            ._companyRetrievalService
            .CompanyExistsAsync((Guid)this.Input.CompanyId);

        if (!companyExists)
        {
            ModelState.AddModelError(nameof(Input.CompanyId), CompanyDoesNotExistMessage);
        }

        if (!ModelState.IsValid)
        {
            List<AllCompaniesListViewModel> companies = await this
                ._companyRetrievalService
                .GetAllListAsync();

            Input.Roles = _roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Name));
            Input.Companies = companies.Select(c => new SelectListItem(c.Name, c.Id.ToString()));

            return this.Page();
        }

        ApplicationUser user = CreateUser();

        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

        user.FirstName = Input.FirstName;
        user.LastName = Input.LastName;
        user.StreetAddress = Input.StreetAddress;
        user.City = Input.City;
        user.State = Input.State;
        user.PostalCode = Input.PostalCode;
        user.PhoneNumber = Input.PhoneNumber;

        if (Input.Role == CompanyRole)
        {
            user.CompanyId = Input.CompanyId;
        }

        var result = await _userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");

            if (!string.IsNullOrWhiteSpace(Input.Role))
            {
                await _userManager.AddToRoleAsync(user, Input.Role);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, CustomerRole);
            }

            string userId = await _userManager.GetUserIdAsync(user);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme)!;

            string mainPageUrl = Url.Page(
                "/Home/Index",
                pageHandler: null,
                values: new { area = "Customer" },
                protocol: Request.Scheme)!;

            await _emailSender.SendEmailAsync(Input.Email,
                EmailConfirmationSubject,
                this.BuildEmailMessage(callbackUrl!));

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            }
            else
            {
                if (!User.IsInRole(AdminRole))
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                {
                    TempData[Success] = UserHasSuccessfullyBeenCreated;
                }

                return LocalRedirect(returnUrl);
            }
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
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
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }

    private string BuildEmailMessage(string callbackUrl)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(string.Format(EmailConfirmationHeaderMessage, this.Input.FirstName));
        sb.AppendLine(string.Format(EmailConfirmationBodyMessage, HtmlEncoder.Default.Encode(callbackUrl)));
        sb.AppendLine(EmailConfirmationFooterMessage);

        return sb.ToString().TrimEnd();
    }
}