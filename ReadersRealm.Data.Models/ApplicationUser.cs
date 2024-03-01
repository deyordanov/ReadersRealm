namespace ReadersRealm.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Common.Constants.ValidationConstants.ApplicationUserValidation;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid();
    }

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

    public Guid? CompanyId { get; set; }

    [ValidateNever]
    [ForeignKey(nameof(CompanyId))]
    public Company? Company { get; set; }
}