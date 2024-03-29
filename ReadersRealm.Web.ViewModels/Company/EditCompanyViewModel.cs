﻿namespace ReadersRealm.Web.ViewModels.Company;

using System.ComponentModel.DataAnnotations;
using static Common.Constants.ValidationConstants.CompanyValidation;
using static Common.Constants.ValidationMessageConstants.CompanyValidationMessages;

public class EditCompanyViewModel
{
    public Guid Id { get; set; }

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