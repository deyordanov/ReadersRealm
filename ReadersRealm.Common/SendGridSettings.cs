namespace ReadersRealm.Common;

using Contracts;

public class SendGridSettings : ISendGridSettings
{
    public required string SecretKey { get; set; }
}