namespace ReadersRealm.Common.Exceptions.Author;

public class AuthorNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested author was not found.";

    public AuthorNotFoundException()
        : this(DefaultMessage) { }
}