﻿namespace ReadersRealm.Common;

using Contracts;

public class StripeSettings : IStripeSettings
{
    public required string SecretKey { get; set; }

    public required string PublishableKey { get; set; }
}