namespace ReadersRealm.Web.ViewModels.Author;

using System.ComponentModel.DataAnnotations;
using Book;
using Data.Models.Enums;
using static Common.Constants.Constants.AuthorConstants;

public class AuthorViewModel
{
    public Guid Id { get; set; }

    [Display(Name = FirstNameDisplay)]
    public required string FirstName { get; set; }

    [Display(Name = MiddleNameDisplay)]
    public string? MiddleName { get; set; }

    [Display(Name = LastNameDisplay)]
    public required string LastName { get; set; }

    public int? Age { get; set; }

    public Gender Gender { get; set; }

    public required string Email { get; set; }

    [Display(Name = PhoneNumberDisplay)]
    public required string PhoneNumber { get; set; }

    public HashSet<BookViewModel> Books { get; set; } = new();
}