﻿namespace ReadersRealm.DataTransferObjects.OrderDetails;

using Book;
using OrderHeader;

public class OrderDetailsDto
{
    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }

    public OrderHeaderDto OrderHeader { get; set; } = null!;

    public Guid BookId { get; set; }

    public BookDto Book { get; set; } = null!;

    public int Count { get; set; }

    public decimal Price { get; set; }
}