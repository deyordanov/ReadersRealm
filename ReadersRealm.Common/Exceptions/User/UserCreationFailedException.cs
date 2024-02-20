namespace ReadersRealm.Common.Exceptions.User;

public class UserCreationFailedException : InvalidOperationException
{
    private const string DefaultMessage = "The user creation failed!";

    public UserCreationFailedException()
        : base(DefaultMessage) { }

    public UserCreationFailedException(string message)
        : base(message) { }
}