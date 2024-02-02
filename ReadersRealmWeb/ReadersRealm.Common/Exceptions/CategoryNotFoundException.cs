namespace ReadersRealm.Common.Exceptions;

public class CategoryNotFoundException : Exception
{
    private const string DefaultMessage = "Such a category was not found!";

    public CategoryNotFoundException()
     : base(DefaultMessage) { }

    public CategoryNotFoundException(string message)
        : base(message) { }
}