namespace ReadersRealm.Common.Exceptions;

public class ShoppingCartNotFoundException : ApplicationException
{
    private const string DefaultMessage = "Such a shopping cart was not found!";

    public ShoppingCartNotFoundException()
        : base(DefaultMessage) { }

    public ShoppingCartNotFoundException(string message)
        : base(message) { }
}