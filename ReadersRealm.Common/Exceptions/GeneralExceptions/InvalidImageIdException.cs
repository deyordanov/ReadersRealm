namespace ReadersRealm.Common.Exceptions.GeneralExceptions;

public class InvalidImageIdException : Exception
{
    private const string DefaultMessage = "The provided image id is not valid!";

    public InvalidImageIdException()
        : base(DefaultMessage) { }

    public InvalidImageIdException(string message)
        : base(message) { }
}