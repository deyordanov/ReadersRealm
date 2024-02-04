﻿using ReadersRealm.Data.Models.Enums;

namespace ReadersRealm.ViewModels.Book;

using Data.Models;
using ReadersRealm.ViewModels.Category;
using ReadersRealm.Web.ViewModels.Author;
using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstants.Book;
using static Common.ValidationMessages.Book;

public class EditBookViewModel
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

    public BookCover? BookCover { get; set; }

    [Required(ErrorMessage = BookUsedRequiredMessage)]
    public bool Used { get; set; }

    public Guid AuthorId { get; set; }

    public Author? Author { get; set; }

    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public required IEnumerable<AllAuthorsListViewModel>? AuthorsList { get; set; }

    public required IEnumerable<AllCategoriesListViewModel>? CategoriesList { get; set; }
}