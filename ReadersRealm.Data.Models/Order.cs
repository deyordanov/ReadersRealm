﻿namespace ReadersRealm.Data.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public Order()
    {
        Id = Guid.NewGuid();
        OrderDetailsList = new HashSet<OrderDetails>();
    }

    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }
    
    [ForeignKey(nameof(OrderHeaderId))]
    public OrderHeader OrderHeader { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public IEnumerable<OrderDetails> OrderDetailsList { get; set; }
}