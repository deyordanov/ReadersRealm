﻿namespace ReadersRealm.Common.Exceptions;

public class BookNotFoundException : ApplicationException
{
    private const string DefaultMessage = "The requested book was not found.";

    public BookNotFoundException()
        : base(DefaultMessage) { }

    public BookNotFoundException(string message)
        : base(message) { }
}