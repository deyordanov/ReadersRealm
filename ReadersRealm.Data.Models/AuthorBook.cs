namespace ReadersRealm.Data.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Represents the many-to-many relationship between Authors and Books in Readers Realm.
/// This entity facilitates the association of books with multiple authors and vice versa,
/// allowing for a more flexible representation of authorship.
/// </summary>
/// <remarks>
/// The AuthorBook entity includes foreign keys for both the Author and Book entities,
/// establishing a many-to-many relationship between them. Each instance of AuthorBook represents
/// a link between a single Author and a single Book, enabling authors to be associated with multiple books
/// and books to be associated with multiple authors.
/// </remarks>
/// <example>
/// Example usage:
/// var authorBook = new AuthorBook 
/// { 
///     AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
///     BookId = Guid.Parse("F8F6E08B-6876-4A58-AE56-3C5BCAC927A7")
/// };
/// </example>
[PrimaryKey(nameof(AuthorId), nameof(BookId))]
public class AuthorBook
{
    /// <summary>
    /// Gets or sets the unique identifier for the Author associated with the Book.
    /// </summary>
    /// <value>The unique identifier of the Author.</value>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the Author entity. This is a required navigation property that links to the AuthorId.
    /// </summary>
    /// <value>The Author associated with the Book.</value>
    [Required]
    [ForeignKey(nameof(AuthorId))]
    public Author Author { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the Book associated with the Author.
    /// </summary>
    /// <value>The unique identifier of the Book.</value>
    public Guid BookId { get; set; }

    /// <summary>
    /// Gets or sets the Book entity. This is a required navigation property that links to the BookId.
    /// </summary>
    /// <value>The Book associated with the Author.</value>
    [Required]
    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; }
}
