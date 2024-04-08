namespace ReadersRealm.Common.Exceptions.Book;

public class BookNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested book was not found.";

    public BookNotFoundException()
        : this(DefaultMessage) { }
}