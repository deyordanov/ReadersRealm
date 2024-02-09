namespace ReadersRealm.Data.Models;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class Order
{
    public Order()
    {
        this.OrderDetails = new HashSet<OrderDetails>();
    }

    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }
    
    [ForeignKey(nameof(OrderHeaderId))]
    public OrderHeader OrderHeader { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public IEnumerable<OrderDetails> OrderDetails { get; set; }
}