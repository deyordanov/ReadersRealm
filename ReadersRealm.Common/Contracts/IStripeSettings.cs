namespace ReadersRealm.Common.Contracts;

public interface IStripeSettings
{
    public string SecretKey { get; }
    public string PublishableKey { get; }
}