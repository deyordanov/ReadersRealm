using System.ComponentModel.DataAnnotations;

namespace ReadersRealm.ViewModels.Company;
using static Common.Constants.ValidationConstants.Company;
using static Common.Constants.ValidationMessageConstants.Company;

public class CreateCompanyViewModel
{
    [Required(ErrorMessage = CompanyNameRequiredMessage)]
    [StringLength(CompanyNameMaxLength,
        MinimumLength = CompanyNameMinLength,
        ErrorMessage = CompanyNameLengthMessage)]
    public required string Name { get; set; }

    [Required(ErrorMessage = CompanyUicRequiredMessage)]
    [StringLength(CompanyUicMaxLength,
        MinimumLength = CompanyUicMinLength,
        ErrorMessage = CompanyUicLengthMessage)]
    [Display(Name = "Unified Identification Code")]
    public required string UIC { get; set; }

    [Required(ErrorMessage = CompanyEmailRequiredMessage)]
    [StringLength(CompanyEmailMaxLength,
        MinimumLength = CompanyEmailMinLength,
        ErrorMessage = CompanyEmailLengthMessage)]
    public required string Email { get; set; }

    [Display(Name = "Street Address")]
    [StringLength(CompanyStreetAddressMaxLength,
        MinimumLength = CompanyStreetAddressMinLength,
        ErrorMessage = CompanyStreetAddressLengthMessage)]
    public string? StreetAddress { get; set; }

    [StringLength(CompanyCityMaxLength,
        MinimumLength = CompanyCityMinLength,
        ErrorMessage = CompanyCityLengthMessage)]
    public string? City { get; set; }

    [StringLength(CompanyStateMaxLength,
        MinimumLength = CompanyStateMinLength,
        ErrorMessage = CompanyStateLengthMessage)]
    public string? State { get; set; }

    [StringLength(CompanyPostalCodeMaxLength,
        MinimumLength = CompanyPostalCodeMinLength,
        ErrorMessage = CompanyPostalCodeLengthMessage)]
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    [StringLength(CompanyPhoneNumberMaxLength,
        MinimumLength = CompanyPhoneNumberMinLength,
        ErrorMessage = CompanyPhoneNumberLengthMessage)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
}