namespace ReadersRealm.Common.Exceptions.Category;

public class CategoryNotFoundException(string message) : BaseNotFoundException(message)
{
    private const string DefaultMessage = "The requested category was not found.";

    public CategoryNotFoundException()
     : this(DefaultMessage) { }
}