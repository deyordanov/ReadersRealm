namespace ReadersRealm.Web.ViewModels.ApplicationUser;

using System.ComponentModel.DataAnnotations;
using Common.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Common.Constants.Constants.ApplicationUser;

public class RolesApplicationUserViewModel
{
    public RolesApplicationUserViewModel()
    {
        this.OldRoles = new List<string>();
        this.NewRoles = new List<string>();
        this.AllRoles = new List<string>();
    }

    public required Guid Id { get; set; }

    [Display(Name = FirstNameDisplay)]
    public required string FirstName { get; set; }

    [Display(Name = LastNameDisplay)]
    public required string LastName { get; set; }

    [ValidateNever]
    public IList<string> OldRoles { get; set; }

    [Display(Name = UserRolesDisplay)]
    [ValidateNever]
    public IList<string> NewRoles { get; set; }

    [ValidateNever]
    public IList<string> AllRoles { get; set; }
}