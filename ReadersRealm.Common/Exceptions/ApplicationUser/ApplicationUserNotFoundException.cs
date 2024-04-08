namespace ReadersRealm.Common.Exceptions.ApplicationUser;

public class ApplicationUserNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested user was not found.";

    public ApplicationUserNotFoundException()
        : this(DefaultMessage) { }
}