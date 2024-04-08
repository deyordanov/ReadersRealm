// ReSharper disable InconsistentNaming
namespace ReadersRealm.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.Constants.ValidationConstants.CompanyValidation;

public class Company
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(CompanyNameMaxLength)]
    public required string Name { get; set; }

    [Required]
    [StringLength(CompanyUicMaxLength)]
    public required string UIC { get; set; }

    [Required]
    [StringLength(CompanyEmailMaxLength)]
    public required string Email { get; set; }

    [StringLength(CompanyStreetAddressMaxLength)]
    public string? StreetAddress { get; set; }

    [StringLength(CompanyCityMaxLength)]
    public string? City { get; set; }

    [StringLength(CompanyStateMaxLength)]
    public string? State { get; set; }

    [StringLength(CompanyPostalCodeMaxLength)]
    public string? PostalCode { get; set; }

    [StringLength(CompanyPhoneNumberMaxLength)]
    public string? PhoneNumber { get; set; }
}