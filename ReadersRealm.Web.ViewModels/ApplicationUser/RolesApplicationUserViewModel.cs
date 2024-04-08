namespace ReadersRealm.Web.ViewModels.ApplicationUser;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Constants;
using Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Common.Constants.Constants.ApplicationUserConstants;

public class RolesApplicationUserViewModel
{
    public required Guid Id { get; set; }

    [Display(Name = FirstNameDisplay)]
    [ValidateNever]
    public required string FirstName { get; set; }

    [Display(Name = LastNameDisplay)]
    [ValidateNever]
    public required string LastName { get; set; }

    [ValidateNever]
    public IList<string> OldRoles { get; set; } = new List<string>();

    [Display(Name = UserRolesDisplay)]
    [ValidateNever]
    public IList<string> NewRoles { get; set; } = new List<string>();

    [ValidateNever]
    public IList<string> AllRoles { get; set; } = new List<string>();

    [ValidateNever]
    public IList<SelectListItem> Companies { get; set; } = new List<SelectListItem>();

    [Display(Name = CompanyIdDisplay)]
    public Guid? CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company? Company { get; set; }
}