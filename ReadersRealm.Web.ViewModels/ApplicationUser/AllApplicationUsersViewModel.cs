namespace ReadersRealm.Web.ViewModels.ApplicationUser;

using System.ComponentModel.DataAnnotations.Schema;
using Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class AllApplicationUsersViewModel
{
    public AllApplicationUsersViewModel()
    {
        this.Roles = new List<string>();
    }

    public required Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Email { get; set; }

    public required bool IsLocked { get; set; }

    public required IList<string> Roles { get; set; }

    public Guid? CompanyId { get; set; }

    [ValidateNever]
    [ForeignKey(nameof(CompanyId))]
    public Company? Company { get; set; }
}