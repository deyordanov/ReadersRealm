namespace ReadersRealm.ViewModels.ShoppingCart;

using ApplicationUser;
using Book;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Common.Constants.ValidationConstants.ShoppingCart;
using static Common.Constants.ValidationMessageConstants.ShoppingCart;

public class ShoppingCartViewModel
{
    public ShoppingCartViewModel()
    {
        this.Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    [ValidateNever]
    public DetailsBookViewModel Book { get; set; }

    [Required]
    [Range(ShoppingCartCountMinRange, 
        ShoppingCartCountMaxRange,
        ErrorMessage = ShoppingCartCountRangeMessage)]
    public int Count { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(ApplicationUserId))]
    [ValidateNever]
    public ApplicationUserViewModel ApplicationUser { get; set; }

    [ValidateNever]
    public decimal TotalPrice { get; set; }
}