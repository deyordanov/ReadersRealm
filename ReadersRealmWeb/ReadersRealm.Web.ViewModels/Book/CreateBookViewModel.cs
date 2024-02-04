namespace ReadersRealm.ViewModels.Book;

using Category;
using Data.Models;
using ReadersRealm.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using Web.ViewModels.Author;
using static ReadersRealm.Common.Constants.ValidationConstants.Book;
using static ReadersRealm.Common.Constants.ValidationMessageConstants.Book;

public class CreateBookViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = BookTitleRequiredMessage)]
    [StringLength(BookTitleMaxLength,
        MinimumLength = BookTitleMinLength,
        ErrorMessage = BookTitleLengthMessage)]
    public required string Title { get; set; }

    [StringLength(BookDescriptionMaxLength,
        MinimumLength = BookDescriptionMinLength,
        ErrorMessage = BookDescriptionLengthMessage)]
    public string? Description { get; set; }

    [Required(ErrorMessage = BookIsbnRequiredMessage)]
    [StringLength(BookIsbnMaxLength,
        MinimumLength = BookIsbnMinLength,
        ErrorMessage = BookIsbnLengthMessage)]
    public required string ISBN { get; set; }

    [Required(ErrorMessage = BookPriceRequiredMessage)]
    [Range(BookPriceMinRange,
        BookPriceMaxRange,
        ErrorMessage = BookPriceRangeMessage)]
    public decimal Price { get; set; }

    [Range(BookPagesMinRange,
        BookPriceMaxRange,
        ErrorMessage = BookPagesRangeMessage)]
    public int? Pages { get; set; }

    [Display(Name = "Book Cover")]
    public BookCover? BookCover { get; set; }

    [Required(ErrorMessage = BookUsedRequiredMessage)]
    public bool Used { get; set; }

    [Display(Name = "Image")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Author")]
    public Guid AuthorId { get; set; }

    public Author? Author { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public required IEnumerable<AllAuthorsListViewModel>? AuthorsList { get; set; }

    public required IEnumerable<AllCategoriesListViewModel>? CategoriesList { get; set; }
}