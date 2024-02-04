﻿namespace ReadersRealm.ViewModels.Book;

using System.ComponentModel.DataAnnotations;
using Data.Models;
using ReadersRealm.Data.Models.Enums;

public class DeleteBookViewModel
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
    public required string ImageUrl { get; set; }

    [Display(Name = "Author")]
    public Guid AuthorId { get; set; }

    public Author Author { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public Category Category { get; set; }
}