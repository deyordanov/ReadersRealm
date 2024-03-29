﻿namespace ReadersRealm.Common.Exceptions.ShoppingCart;

public class ShoppingCartNotFoundException : BaseNotFoundException
{
    private const string DefaultMessage = "The requested shopping cart was not found.";

    public ShoppingCartNotFoundException()
        : base(DefaultMessage) { }

    public ShoppingCartNotFoundException(string message)
        : base(message) { }
}