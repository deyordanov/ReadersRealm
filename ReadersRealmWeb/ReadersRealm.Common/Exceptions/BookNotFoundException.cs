namespace ReadersRealm.Common.Exceptions;

public class BookNotFoundException : Exception
{
    private const string DefaultMessage = "Such a book was not found!";

    public BookNotFoundException()
        : base(DefaultMessage) { }

    public BookNotFoundException(string message)
        : base(message) { }
}