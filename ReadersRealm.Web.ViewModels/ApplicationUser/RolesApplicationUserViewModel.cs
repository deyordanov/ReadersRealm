namespace ReadersRealm.Web.ViewModels.ApplicationUser;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Constants;
using Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Common.Constants.Constants.ApplicationUser;

public class RolesApplicationUserViewModel
{
    public RolesApplicationUserViewModel()
    {
        this.OldRoles = new List<string>();
        this.NewRoles = new List<string>();
        this.AllRoles = new List<string>();
        this.Companies = new List<SelectListItem>();
    }

    public required Guid Id { get; set; }

    [Display(Name = FirstNameDisplay)]
    [ValidateNever]
    public required string FirstName { get; set; }

    [Display(Name = LastNameDisplay)]
    [ValidateNever]
    public required string LastName { get; set; }

    [ValidateNever]
    public IList<string> OldRoles { get; set; }

    [Display(Name = UserRolesDisplay)]
    [ValidateNever]
    public IList<string> NewRoles { get; set; }

    [ValidateNever]
    public IList<string> AllRoles { get; set; }

    [ValidateNever]
    public IList<SelectListItem> Companies { get; set; }

    [Display(Name = CompanyIdDisplay)]
    public Guid? CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company? Company { get; set; }
}