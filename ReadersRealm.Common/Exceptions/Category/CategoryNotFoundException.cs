namespace ReadersRealm.Common.Exceptions.Category;

public class CategoryNotFoundException : ApplicationException
{
    private const string DefaultMessage = "The requested category was not found.";

    public CategoryNotFoundException()
     : base(DefaultMessage) { }

    public CategoryNotFoundException(string message)
        : base(message) { }
}