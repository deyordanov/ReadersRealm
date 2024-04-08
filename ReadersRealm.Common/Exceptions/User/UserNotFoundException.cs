namespace ReadersRealm.Common.Exceptions.User;

public class UserNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested user was not found.";

    public UserNotFoundException()
        : this(DefaultMessage) { }
}