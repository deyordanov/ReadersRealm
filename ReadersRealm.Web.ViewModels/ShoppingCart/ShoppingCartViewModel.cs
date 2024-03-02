namespace ReadersRealm.Web.ViewModels.ShoppingCart;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationUser;
using Book;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Common.Constants.ValidationConstants.ShoppingCartValidation;
using static Common.Constants.ValidationMessageConstants.ShoppingCartValidationMessages;

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
    public DetailsBookViewModel Book { get; set; } = null!;

    [Required]
    [Range(ShoppingCartCountMinRange, 
        ShoppingCartCountMaxRange,
        ErrorMessage = ShoppingCartCountRangeMessage)]
    public int Count { get; set; }

    public Guid ApplicationUserId { get; set; }
    
    [ForeignKey(nameof(ApplicationUserId))]
    [ValidateNever]
    public ApplicationUserViewModel ApplicationUser { get; set; } = null!;

    [ValidateNever]
    public decimal TotalPrice { get; set; }
}