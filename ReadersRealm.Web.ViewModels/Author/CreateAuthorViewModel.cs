namespace ReadersRealm.Web.ViewModels.Author;

using System.ComponentModel.DataAnnotations;
using Book;
using Data.Models.Enums;
using static Common.Constants.ValidationConstants.AuthorValidation;
using static Common.Constants.ValidationMessageConstants.AuthorValidationMessages;
using static Common.Constants.Constants.AuthorConstants;

public class CreateAuthorViewModel
{
    [Required(ErrorMessage = AuthorFirstNameRequiredMessage)]
    [StringLength(AuthorFirstNameMaxLength,
        MinimumLength = AuthorFirstNameMinLength,
        ErrorMessage = AuthorFirstNameLengthMessage)]
    [Display(Name = FirstNameDisplay)]
    public required string FirstName { get; set; }

    [StringLength(AuthorMiddleNameMaxLength,
        MinimumLength = AuthorMiddleNameMinLength,
        ErrorMessage = AuthorMiddleNameLengthMessage)]
    [Display(Name = MiddleNameDisplay)]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = AuthorLastNameRequiredMessage)]
    [StringLength(AuthorLastNameMaxLength,
        MinimumLength = AuthorLastNameMinLength,
        ErrorMessage = AuthorLastNameLengthMessage)]
    [Display(Name = LastNameDisplay)]
    public required string LastName { get; set; }

    [Range(AuthorAgeMinRange, 
        AuthorAgeMaxRange, 
        ErrorMessage = AuthorAgeRangeMessage)]
    public int? Age { get; set; }

    [Required(ErrorMessage = AuthorGenderRequiredMessage)]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = AuthorEmailRequiredMessage)]
    [StringLength(AuthorEmailMaxLength,
        MinimumLength = AuthorEmailMinLength,
        ErrorMessage = AuthorEmailLengthMessage)]
    public required string Email { get; set; }

    [Required(ErrorMessage = AuthorPhoneNumberRequiredMessage)]
    [StringLength(AuthorPhoneNumberMaxLength,
        MinimumLength = AuthorPhoneNumberMinLength,
        ErrorMessage = AuthorPhoneNumberLengthMessage)]
    [Display(Name = PhoneNumberDisplay)]
    public required string PhoneNumber { get; set; }

    public HashSet<BookViewModel> Books { get; set; } = new();
}