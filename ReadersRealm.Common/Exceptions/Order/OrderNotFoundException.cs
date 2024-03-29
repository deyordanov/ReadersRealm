﻿namespace ReadersRealm.Common.Exceptions.Order;

public class OrderNotFoundException : BaseNotFoundException
{
    private const string DefaultMessage = "The requested order was not found!";

    public OrderNotFoundException()
        : base(DefaultMessage) { }

    public OrderNotFoundException(string message)
        : base(message) { }
}