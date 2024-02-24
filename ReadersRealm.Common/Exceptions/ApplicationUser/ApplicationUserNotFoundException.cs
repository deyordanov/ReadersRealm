﻿namespace ReadersRealm.Common.Exceptions.ApplicationUser;

public class ApplicationUserNotFoundException : Exception
{
    private const string DefaultMessage = "The requested user was not found.";

    public ApplicationUserNotFoundException()
        : base(DefaultMessage) { }

    public ApplicationUserNotFoundException(string message)
        : base(message) { }
}