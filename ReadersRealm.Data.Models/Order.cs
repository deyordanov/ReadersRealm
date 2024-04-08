namespace ReadersRealm.Data.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OrderHeaderId { get; set; }
    
    [ForeignKey(nameof(OrderHeaderId))]
    public OrderHeader OrderHeader { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public IEnumerable<OrderDetails> OrderDetailsList { get; set; } = new HashSet<OrderDetails>();
}