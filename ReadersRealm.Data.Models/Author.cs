namespace ReadersRealm.Data.Models;

using Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static ReadersRealm.Common.Constants.ValidationConstants.Author;

/// <summary>
/// Represents an author in Readers Realm.
/// Authors are integral to the platform, creating the content that populates its library.
/// </summary>
/// <remarks>
/// This model includes the author's personal details such as name, age, gender, email, and phone number.
/// It also includes a collection of books, representing the works associated with the author.
/// </remarks>
/// <example>
/// Example usage:
/// Author newAuthor = new Author
/// {
///     FirstName = "Jane",
///     LastName = "Doe",
///     Email = "janedoe@example.com",
///     Books = new HashSet<Book>
///     {
///         new Book { Title = "The First Adventure", ISBN = "123-456-789", Price = 19.99 }
///     }
/// };
/// </example>
[Comment("Readers Realm Author")]
public class Author
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Author"/> class.
    /// Sets the Id property to a new GUID and initializes the Books collection.
    /// </summary>
    public Author()
    {
        this.Id = Guid.NewGuid();
        this.Books = new HashSet<Book>();
    }

    /// <summary>
    /// Gets or sets the unique identifier for the Author.
    /// </summary>
    /// <value>The Author's unique identifier.</value>
    [Key]
    [Comment("Author Identifier")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the first name of the Author.
    /// The first name is a required field and has a maximum length defined in AuthorFirstNameMaxLength.
    /// </summary>
    /// <value>The first name of the Author.</value>
    [Required]
    [StringLength(AuthorFirstNameMaxLength)]
    [Comment("Author First Name")]
    public required string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the middle name of the Author.
    /// The middle name is optional and has a maximum length defined in AuthorMiddleNameMaxLength.
    /// </summary>
    /// <value>The middle name of the Author.</value>
    [StringLength(AuthorMiddleNameMaxLength)]
    [Comment("Author Middle Name")]
    public string? MiddleName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the Author.
    /// The last name is a required field and has a maximum length defined in AuthorLastNameMaxLength.
    /// </summary>
    /// <value>The last name of the Author.</value>
    [Required]
    [StringLength(AuthorLastNameMaxLength)]
    [Comment("Author Last Name")]
    public required string LastName { get; set; }

    /// <summary>
    /// Gets or sets the age of the Author.
    /// The age is optional.
    /// </summary>
    /// <value>The age of the Author.</value>
    [Comment("Author Age")]
    public int? Age { get; set; }

    /// <summary>
    /// Gets or sets the gender of the Author, represented by the Gender enum.
    /// </summary>
    /// <value>The gender of the Author.</value>
    [Required]
    [Comment("Author Gender")]
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the email address of the Author.
    /// The email is optional and has a maximum length defined in AuthorEmailMaxLength.
    /// </summary>
    /// <value>The email address of the Author.</value>
    [Required]
    [StringLength(AuthorEmailMaxLength)]
    [Comment("Author Email")]
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the Author.
    /// The phone number is a required field and has a maximum length defined in AuthorPhoneNumberMaxLength.
    /// </summary>
    /// <value>The phone number of the Author.</value>
    [Required]
    [StringLength(AuthorPhoneNumberMaxLength)]
    [Comment("Author Phone Number")]
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the collection of books written by the Author.
    /// This property represents the relationship between authors and their books.
    /// </summary>
    /// <value>The collection of books associated with the Author.</value>
    [Comment("Author's Books")]
    public HashSet<Book> Books { get; set; }
}
