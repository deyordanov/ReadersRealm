namespace ReadersRealm.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Common.Constants.ValidationConstants.OrderDetails;

public class OrderDetails
{
    public OrderDetails()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }

    [ForeignKey(nameof(OrderHeaderId))]
    [ValidateNever]
    public OrderHeader OrderHeader { get; set; } = null!;

    public Guid BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    [ValidateNever]
    public Book Book { get; set; } = null!;

    public int Count { get; set; }

    [Column(TypeName = OrderDetailsPriceColumnType)]
    public decimal Price { get; set; }

    public Guid OrderId { get; set; }


    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
}