namespace ReadersRealm.Common;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static Common.Constants.ValidationConstants.ApplicationUser;

public class ApplicationUser : IdentityUser
{

    [Required]
    [StringLength(ApplicationUserFirstNameMaxLength)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(ApplicationUserLastNameMaxLength)]
    public required string LastName { get; set; }

    [StringLength(ApplicationUserStreetAddressMaxLength)]
    public string? StreetAddress { get; set; }

    [StringLength(ApplicationUserCityMaxLength)]
    public string? City { get; set; }

    [StringLength(ApplicationUserStateMaxLength)]
    public string? State { get; set; }

    [StringLength(ApplicationUserPostalCodeMaxLength)]
    public string? PostalCode { get; set; }
}