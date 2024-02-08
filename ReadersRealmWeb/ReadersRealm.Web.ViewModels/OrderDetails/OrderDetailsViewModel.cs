using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadersRealm.Web.ViewModels.OrderDetails;

using Data.Models;

public class OrderDetailsViewModel
{
    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }

    [ValidateNever]
    public OrderHeader OrderHeader { get; set; }

    public Guid BookId { get; set; }

    [ValidateNever]
    public Book Book { get; set; }

    public int Count { get; set; }

    public decimal Price { get; set; }
}