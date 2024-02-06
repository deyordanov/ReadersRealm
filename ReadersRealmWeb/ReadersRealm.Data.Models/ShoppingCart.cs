namespace ReadersRealm.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class ShoppingCart
{
    public ShoppingCart()
    {
        this.Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; }

    [Required]
    public int Count { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser ApplicationUser { get; set; }
}