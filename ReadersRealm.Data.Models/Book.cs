namespace ReadersRealm.Data.Models;

using Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ReadersRealm.Common.Constants.ValidationConstants.BookValidation;

/// <summary>
/// Represents a book in Readers Realm.
/// Books are fundamental entities in the platform, encapsulating various literary works.
/// </summary>
/// <remarks>
/// Each book has a unique ISBN, a title, and is linked to an author. 
/// Additional attributes like description, price, page count, cover type, and condition (new/used) are also present.
/// </remarks>
/// <example>
/// Example usage:
/// Book sampleBook = new Book { Title = "Sample Title", ISBN = "1234567890", AuthorId = Guid.NewGuid() };
/// </example>
[Comment("Readers Realm Book")]
public class Book
{
    /// <summary>
    /// Gets or sets the unique identifier for the Book.
    /// </summary>
    /// <value>The Book's unique identifier.</value>
    [Key]
    [Comment("Book Identifier")]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the title of the Book.
    /// The title is a required field and has a maximum length defined in BookTitleMaxLength.
    /// </summary>
    /// <value>The title of the Book.</value>
    [Required]
    [StringLength(BookTitleMaxLength)]
    [Comment("Book Title")]
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the Book.
    /// The description is optional and has a maximum length defined in BookDescriptionMaxLength.
    /// </summary>
    /// <value>The description of the Book.</value>
    [StringLength(BookDescriptionMaxLength)]
    [Comment("Book Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the ISBN of the Book.
    /// The ISBN is a unique identifier for books, required and has a maximum length defined in BookIsbnMaxLength.
    /// </summary>
    /// <value>The ISBN of the Book.</value>
    [Required]
    [StringLength(BookIsbnMaxLength)]
    [Comment("Book's International Standard Book Number")]
    public required string ISBN { get; set; }

    /// <summary>
    /// Gets or sets the price of the Book.
    /// </summary>
    /// <value>The price of the Book.</value>
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Comment("Book Price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the number of pages in the Book.
    /// </summary>
    /// <value>The number of pages in the Book.</value>
    [Comment("Book Page Count")]
    public int? Pages { get; set; }

    /// <summary>
    /// Gets or sets the type of cover of the Book, represented by the BookCover enum.
    /// </summary>
    /// <value>The type of cover of the Book.</value>
    [Comment("Book Cover Type")]
    public BookCover? BookCover { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the Book is used.
    /// </summary>
    /// <value><c>true</c> if the Book is used; otherwise, <c>false</c>.</value>
    [Comment("Book Condition")]
    public bool Used { get; set; }

    [Comment("Book Image Id")]
    [StringLength(BookImageIdMaxLength)]
    public string? ImageId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the Author of the Book.
    /// </summary>
    /// <value>The Author's unique identifier.</value>
    [Required]
    [Comment("Author Identifier")]
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the Author of the Book.
    /// This is a required navigation property linked to the AuthorId.
    /// </summary>
    /// <value>The Author of the Book.</value>
    [Required]
    [ForeignKey(nameof(AuthorId))]
    [Comment("Book's Author")]
    public Author Author { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the Category of the Book.
    /// This property represents the foreign key in the database that links the book to its category,
    /// enabling a hierarchical organization of books by their subject matter or genre.
    /// </summary>
    /// <value>The Category's unique identifier.</value>
    [Required]
    [Comment("Category Identifier")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the Category of the Book.
    /// This is a navigation property that allows for the direct access to the Category entity
    /// associated with this book, facilitating queries that involve book categorization.
    /// </summary>
    /// <value>The Category to which the Book belongs.</value>
    [ForeignKey(nameof(CategoryId))]
    [Comment("Book's Category")]
    public Category Category { get; set; }
}
