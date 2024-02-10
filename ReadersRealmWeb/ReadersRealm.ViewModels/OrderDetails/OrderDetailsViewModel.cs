namespace ReadersRealm.ViewModels.OrderDetails;

using Book;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OrderHeader;

public class OrderDetailsViewModel
{
    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }

    [ValidateNever]
    public OrderHeaderViewModel OrderHeader { get; set; } = null!;

    public Guid BookId { get; set; }

    [ValidateNever]
    public BookViewModel Book { get; set; } = null!;

    public int Count { get; set; }

    public decimal Price { get; set; }
}