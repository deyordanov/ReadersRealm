namespace ReadersRealm.Common.Exceptions.Services;

public class ServiceTypeNotFoundException : InvalidOperationException
{
    private const string DefaultMessage = "The provided service type is invalid!";

    public ServiceTypeNotFoundException()
        : base(DefaultMessage) { }

    public ServiceTypeNotFoundException(string message)
        : base(message) { }
}