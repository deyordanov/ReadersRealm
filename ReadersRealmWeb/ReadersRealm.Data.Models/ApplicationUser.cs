namespace ReadersRealm.Common;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static Common.Constants.ValidationConstants.ApplicationUser;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(ApplicationUserFullNameMaxLength)]
    public required string FullName { get; set; }

    [StringLength(ApplicationUserStreetAddressMaxLength)]
    public string? StreetAddress { get; set; }

    [StringLength(ApplicationUserCityMaxLength)]
    public string? City { get; set; }

    [StringLength(ApplicationUserStateMaxLength)]
    public string? State { get; set; }

    [StringLength(ApplicationUserPostalCodeMaxLength)]
    public string? PostalCode { get; set; }
}