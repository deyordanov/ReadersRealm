namespace ReadersRealm.Common.Exceptions.ApplicationUser;

public class ApplicationUserNotFoundException : BaseNotFoundException
{
    private const string DefaultMessage = "The requested user was not found.";

    public ApplicationUserNotFoundException()
        : base(DefaultMessage) { }

    public ApplicationUserNotFoundException(string message)
        : base(message) { }
}