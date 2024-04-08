namespace ReadersRealm.Common.Exceptions.Services;

public class ServiceTypeNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The provided service type is invalid!";

    public ServiceTypeNotFoundException()
        : this(DefaultMessage) { }
}