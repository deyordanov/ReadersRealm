namespace ReadersRealm.Common.Exceptions.User;

public class UserNotFoundException : ApplicationException
{
    private const string DefaultMessage = "The requested user was not found.";

    public UserNotFoundException()
        : base(DefaultMessage) { }

    public UserNotFoundException(string message)
        : base(message) { }
}