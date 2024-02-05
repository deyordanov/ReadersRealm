﻿namespace ReadersRealm.ViewModels.Book;

using Data.Models;
using Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class DetailsBookViewModel
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required string ISBN { get; set; }

    public decimal Price { get; set; }

    public int? Pages { get; set; }

    [Display(Name = "Book Cover")]
    public BookCover? BookCover { get; set; }

    public bool Used { get; set; }

    [Display(Name = "Image")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Author")]
    public Guid AuthorId { get; set; }

    public Author Author { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public Category Category { get; set; }
}