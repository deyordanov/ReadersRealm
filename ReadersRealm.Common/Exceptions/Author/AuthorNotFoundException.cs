namespace ReadersRealm.Common.Exceptions.Author;

public class AuthorNotFoundException : BaseNotFoundException
{
    private const string DefaultMessage = "The requested author was not found.";

    public AuthorNotFoundException()
        : base(DefaultMessage) { }

    public AuthorNotFoundException(string message)
        : base(message) { }
}