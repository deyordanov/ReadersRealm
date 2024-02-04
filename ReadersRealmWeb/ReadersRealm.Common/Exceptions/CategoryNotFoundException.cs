namespace ReadersRealm.Common.Exceptions;

public class CategoryNotFoundException : ApplicationException
{
    private const string DefaultMessage = "Such a category was not found!";

    public CategoryNotFoundException()
     : base(DefaultMessage) { }

    public CategoryNotFoundException(string message)
        : base(message) { }
}