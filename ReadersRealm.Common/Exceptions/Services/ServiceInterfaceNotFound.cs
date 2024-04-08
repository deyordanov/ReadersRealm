namespace ReadersRealm.Common.Exceptions.Services;

public class ServiceInterfaceNotFound(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "No interface was found for the provided service.";

	public ServiceInterfaceNotFound() 
        : this(DefaultMessage) { }
}