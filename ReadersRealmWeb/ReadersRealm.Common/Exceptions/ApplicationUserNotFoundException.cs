namespace ReadersRealm.Common.Exceptions;

public class ApplicationUserNotFoundException : ApplicationException
{
    private const string DefaultMessage = "The requested user was not found.";

    public ApplicationUserNotFoundException()
        : base(DefaultMessage) { }

    public ApplicationUserNotFoundException(string message)
        : base(message) { }
}