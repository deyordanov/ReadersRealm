namespace ReadersRealm.Web.ViewModels.Identity;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Common.Constants.Constants.RegisterModelConstants;
using static Common.Constants.ValidationConstants.RegisterModelValidation;
using static Common.Constants.ValidationMessageConstants.RegisterModelValidationMessages;

public class RegisterInputViewModel
{
    public RegisterInputViewModel()
    {
        Roles = new List<SelectListItem>();
        Companies = new List<SelectListItem>();
    }

    [Required]
    [EmailAddress]
    [Display(Name = EmailDisplay)]
    public required string Email { get; set; }

    [Required]
    [StringLength(RegisterModelPasswordMaxLength,
        MinimumLength = RegisterModelPasswordMinLength,
        ErrorMessage = RegisterModelPasswordLengthMessage)]
    [DataType(DataType.Password)]
    [Display(Name = PasswordDisplay)]
    public required string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = ConfirmPasswordDisplay)]
    [Compare(nameof(Password),
        ErrorMessage = RegisterModelPasswordCompareMessage)]
    public required string ConfirmPassword { get; set; }

    [Required]
    [Display(Name = FirstNameDisplay)]
    [StringLength(RegisterModelFirstNameMaxLength,
        MinimumLength = RegisterModelFirstNameMinLength,
        ErrorMessage = RegisterModelFirstNameLengthMessage)]
    public required string FirstName { get; set; }

    [Required]
    [Display(Name = LastNameDisplay)]
    [StringLength(RegisterModelLastNameMaxLength,
        MinimumLength = RegisterModelLastNameMinLength,
        ErrorMessage = RegisterModelLastNameLengthMessage)]
    public required string LastName { get; set; }

    [Display(Name = StreetAddressDisplay)]
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

    [Display(Name = PostalCodeDisplay)]
    [StringLength(RegisterModelPostalCodeMaxLength,
        MinimumLength = RegisterModelPostalCodeMinLength,
        ErrorMessage = RegisterModelPostalCodeLengthMessage)]
    public string? PostalCode { get; set; }

    [Display(Name = PhoneNumberDisplay)]
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